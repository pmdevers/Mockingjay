using System;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// If not specified otherwise, the resources are in Dutch.
[assembly: NeutralResourcesLanguage("nl")]

// We don't need to support other languages than C#.
[assembly: CLSCompliant(false)]

// No COM visibility.
// Classes which are exposed to COM must have a parameterless constructor. That
[assembly: ComVisible(false)]

[assembly: InternalsVisibleTo("Application.Tests")]
