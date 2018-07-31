---
title: DotNetStarter - Known Issues
---
# Known Issues

* Apps targeting less than netstandard1.6 need to provide assemblies, as there is no default assembly loader for them.
* Apps targeting less than netstandard1.6 may not perform the shutdown event.
* Import&lt;T> and the default ILocatorAmbient do NOT support scoping for netstandard1.0.