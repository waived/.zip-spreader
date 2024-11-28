──┐  ┬  ┌─┐    ┌──  ┌─┐  ┬──┐  ┌──  ┌─┐  ┌┬─┐
┌─┘  │  ├─┘    └─┐  ├─┘  │─┬┘  ├─   ├─┤   │ │
└──  ┴  ┴      ──┘  ┴    ┴ └   └──  ┴ ┴  ─┴─┘

Overview:
    Simple .net class that scans a Windows NT environment for
    any .zip compressed folders and attempts to add a malicious
    payload.


Requirements:
    The program needs a "System.IO.Compression" dependency which
    can be added to the project by going to...

    Assemblies > Framework > Add > System.IO.Compression

    This class will not function without it.


Considerations:
    By default, this class scans common user folders such as
    the Desktop, Pictures, Documents, Music, Videos, Favorites
    and Downloads folder. Slight modifications can be made in
    effort to scan the entire system, including protected
    folders. UAC elevation may be required in such cases.
