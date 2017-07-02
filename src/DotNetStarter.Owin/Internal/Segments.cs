﻿namespace DotNetStarter.Owin.Internal
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    internal struct HeaderSegment : IEquatable<HeaderSegment>
    {
        private readonly StringSegment _formatting;
        private readonly StringSegment _data;

        // <summary>
        // Initializes a new instance of the <see cref="T:System.Object"/> class.
        // </summary>
        public HeaderSegment(StringSegment formatting, StringSegment data)
        {
            _formatting = formatting;
            _data = data;
        }

        public StringSegment Formatting => _formatting;

        public StringSegment Data => _data;

        #region Equality members

        public bool Equals(HeaderSegment other) => _formatting.Equals(other._formatting) && _data.Equals(other._data);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is HeaderSegment && Equals((HeaderSegment)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_formatting.GetHashCode() * 397) ^ _data.GetHashCode();
            }
        }

        public static bool operator ==(HeaderSegment left, HeaderSegment right) => left.Equals(right);

        public static bool operator !=(HeaderSegment left, HeaderSegment right) => !left.Equals(right);

        #endregion
    }

    internal struct HeaderSegmentCollection : IEnumerable<HeaderSegment>, IEquatable<HeaderSegmentCollection>
    {
        private readonly string[] _headers;

        public HeaderSegmentCollection(string[] headers)
        {
            _headers = headers;
        }

        #region Equality members

        public bool Equals(HeaderSegmentCollection other) => Equals(_headers, other._headers);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is HeaderSegmentCollection && Equals((HeaderSegmentCollection)obj);
        }

        public override int GetHashCode() => (_headers != null ? _headers.GetHashCode() : 0);

        public static bool operator ==(HeaderSegmentCollection left, HeaderSegmentCollection right) => left.Equals(right);

        public static bool operator !=(HeaderSegmentCollection left, HeaderSegmentCollection right) => !left.Equals(right);

        #endregion

        public Enumerator GetEnumerator() => new Enumerator(_headers);

        IEnumerator<HeaderSegment> IEnumerable<HeaderSegment>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        internal struct Enumerator : IEnumerator<HeaderSegment>
        {
            private readonly string[] _headers;
            private int _index;

            private string _header;
            private int _headerLength;
            private int _offset;

            private int _leadingStart;
            private int _leadingEnd;
            private int _valueStart;
            private int _valueEnd;
            private int _trailingStart;

            private Mode _mode;

            private static readonly string[] NoHeaders = new string[0];

            public Enumerator(string[] headers)
            {
                _headers = headers ?? NoHeaders;
                _header = string.Empty;
                _headerLength = -1;
                _index = -1;
                _offset = -1;
                _leadingStart = -1;
                _leadingEnd = -1;
                _valueStart = -1;
                _valueEnd = -1;
                _trailingStart = -1;
                _mode = Mode.Leading;
            }

            private enum Mode
            {
                Leading,
                Value,
                ValueQuoted,
                Trailing,
                Produce,
            }

            private enum Attr
            {
                Value,
                Quote,
                Delimiter,
                Whitespace
            }

            public HeaderSegment Current => new HeaderSegment(new StringSegment(_header, _leadingStart, _leadingEnd - _leadingStart), new StringSegment(_header, _valueStart, _valueEnd - _valueStart));

            object IEnumerator.Current => Current;

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                while (true)
                {
                    if (_mode == Mode.Produce)
                    {
                        _leadingStart = _trailingStart;
                        _leadingEnd = -1;
                        _valueStart = -1;
                        _valueEnd = -1;
                        _trailingStart = -1;

                        if (_offset == _headerLength &&
                            _leadingStart != -1 &&
                            _leadingStart != _offset)
                        {
                            // Also produce trailing whitespace
                            _leadingEnd = _offset;
                            return true;
                        }
                        _mode = Mode.Leading;
                    }

                    // if end of a string
                    if (_offset == _headerLength)
                    {
                        ++_index;
                        _offset = -1;
                        _leadingStart = 0;
                        _leadingEnd = -1;
                        _valueStart = -1;
                        _valueEnd = -1;
                        _trailingStart = -1;

                        // if that was the last string
                        if (_index == _headers.Length)
                        {
                            // no more move nexts
                            return false;
                        }

                        // grab the next string
                        _header = _headers[_index] ?? string.Empty;
                        _headerLength = _header.Length;
                    }
                    while (true)
                    {
                        ++_offset;
                        char ch = _offset == _headerLength ? (char)0 : _header[_offset];
                        Attr attr = char.IsWhiteSpace(ch) ? Attr.Whitespace : ch == '\"' ? Attr.Quote : (ch == ',' || ch == (char)0) ? Attr.Delimiter : Attr.Value;

                        switch (_mode)
                        {
                            case Mode.Leading:
                                switch (attr)
                                {
                                    case Attr.Delimiter:
                                        _leadingEnd = _offset;
                                        _mode = Mode.Produce;
                                        break;
                                    case Attr.Quote:
                                        _leadingEnd = _offset;
                                        _valueStart = _offset;
                                        _mode = Mode.ValueQuoted;
                                        break;
                                    case Attr.Value:
                                        _leadingEnd = _offset;
                                        _valueStart = _offset;
                                        _mode = Mode.Value;
                                        break;
                                    case Attr.Whitespace:
                                        // more
                                        break;
                                }
                                break;
                            case Mode.Value:
                                switch (attr)
                                {
                                    case Attr.Quote:
                                        _mode = Mode.ValueQuoted;
                                        break;
                                    case Attr.Delimiter:
                                        _valueEnd = _offset;
                                        _trailingStart = _offset;
                                        _mode = Mode.Produce;
                                        break;
                                    case Attr.Value:
                                        // more
                                        break;
                                    case Attr.Whitespace:
                                        _valueEnd = _offset;
                                        _trailingStart = _offset;
                                        _mode = Mode.Trailing;
                                        break;
                                }
                                break;
                            case Mode.ValueQuoted:
                                switch (attr)
                                {
                                    case Attr.Quote:
                                        _mode = Mode.Value;
                                        break;
                                    case Attr.Delimiter:
                                        if (ch == (char)0)
                                        {
                                            _valueEnd = _offset;
                                            _trailingStart = _offset;
                                            _mode = Mode.Produce;
                                        }
                                        break;
                                    case Attr.Value:
                                    case Attr.Whitespace:
                                        // more
                                        break;
                                }
                                break;
                            case Mode.Trailing:
                                switch (attr)
                                {
                                    case Attr.Delimiter:
                                        _mode = Mode.Produce;
                                        break;
                                    case Attr.Quote:
                                        // back into value
                                        _trailingStart = -1;
                                        _valueEnd = -1;
                                        _mode = Mode.ValueQuoted;
                                        break;
                                    case Attr.Value:
                                        // back into value
                                        _trailingStart = -1;
                                        _valueEnd = -1;
                                        _mode = Mode.Value;
                                        break;
                                    case Attr.Whitespace:
                                        // more
                                        break;
                                }
                                break;
                        }
                        if (_mode == Mode.Produce)
                        {
                            return true;
                        }
                    }
                }
            }

            public void Reset()
            {
                _index = 0;
                _offset = 0;
                _leadingStart = 0;
                _leadingEnd = 0;
                _valueStart = 0;
                _valueEnd = 0;
            }
        }
    }

    internal struct StringSegment : IEquatable<StringSegment>
    {
        private readonly string _buffer;
        private readonly int _offset;
        private readonly int _count;

        // <summary>
        // Initializes a new instance of the <see cref="T:System.Object"/> class.
        // </summary>
        public StringSegment(string buffer, int offset, int count)
        {
            _buffer = buffer;
            _offset = offset;
            _count = count;
        }

        public string Buffer => _buffer;

        public int Offset => _offset;

        public int Count => _count;

        public string Value => _offset == -1 ? null : _buffer.Substring(_offset, _count);

        public bool HasValue => _offset != -1 && _count != 0 && _buffer != null;

        #region Equality members

        public bool Equals(StringSegment other) => string.Equals(_buffer, other._buffer) && _offset == other._offset && _count == other._count;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is StringSegment && Equals((StringSegment)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (_buffer != null ? _buffer.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ _offset;
                hashCode = (hashCode * 397) ^ _count;
                return hashCode;
            }
        }

        public static bool operator ==(StringSegment left, StringSegment right) => left.Equals(right);

        public static bool operator !=(StringSegment left, StringSegment right) => !left.Equals(right);

        #endregion

        public bool StartsWith(string text, StringComparison comparisonType)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }
            int textLength = text.Length;
            if (!HasValue || _count < textLength)
            {
                return false;
            }

            return string.Compare(_buffer, _offset, text, 0, textLength, comparisonType) == 0;
        }

        public bool EndsWith(string text, StringComparison comparisonType)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }
            int textLength = text.Length;
            if (!HasValue || _count < textLength)
            {
                return false;
            }

            return string.Compare(_buffer, _offset + _count - textLength, text, 0, textLength, comparisonType) == 0;
        }

        public bool Equals(string text, StringComparison comparisonType)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }
            int textLength = text.Length;
            if (!HasValue || _count != textLength)
            {
                return false;
            }

            return string.Compare(_buffer, _offset, text, 0, textLength, comparisonType) == 0;
        }

        public string Substring(int offset, int length) => _buffer.Substring(_offset + offset, length);

        public StringSegment Subsegment(int offset, int length) => new StringSegment(_buffer, _offset + offset, length);

        public override string ToString() => Value ?? string.Empty;
    }
}