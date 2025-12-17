Imports System.Data
Imports System.IO
Imports Microsoft.Win32

Module Program
    Sub Main(args As String())


        Dim Passes As Integer = 5
        Dim maxPasses As Integer = Passes
        Dim sleeptime As Integer = 1000
        Dim popshaderprog As Boolean = False
        If args.Contains("-pop") Then
            popshaderprog = True
        End If


        Console.WriteLine("Automatic shader dump")

        Dim str As New IO.DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))
        str = str.Parent

        Dim locallow As New IO.DirectoryInfo(str.FullName & "\locallow")
        Dim local As New IO.DirectoryInfo(str.FullName & "\local")

        Dim nvpath As String

        Try
            nvpath = Registry.LocalMachine.OpenSubKey("software\\nvidia corporation\\Global\\nvapp").GetValue("FullPath")
        Catch ex As Exception

        End Try




        While Passes > 0

            If locallow.Exists Then
                Console.WriteLine("Checking locallow for shader files")

                Dim cs As New IO.DirectoryInfo(locallow.FullName & "\nvidia\DXCache")
                Console.WriteLine(cs.FullName & " " & If(cs.Exists, "DX found", "DX not found"))

                If cs.Exists And False Then
                    Console.WriteLine("Deleting DX shaders")
                    Dim failname As String = ""
                    Try
                        For Each itm As IO.FileSystemInfo In cs.GetFileSystemInfos
                            failname = itm.Name
                            itm.Delete()

                        Next
                    Catch ex As Exception
                        Console.WriteLine("Failed to delete DX shader :" & failname & " because " & ex.Message)
                    End Try
                End If


                If local.Exists Then
                    Console.WriteLine("Finding NVIDIA shader and OpenGL shaders")
                    Dim nvidcash As New IO.DirectoryInfo(local.FullName & "\nvidia")

                    If nvidcash.Exists Then
                        For Each itm As IO.DirectoryInfo In nvidcash.GetDirectories
                            If itm.Name.ToLower = "dxcache" Then
                                For Each itm2 As IO.FileInfo In itm.GetFiles
                                    Dim failname As String = ""
                                    Console.WriteLine("Deleting " & itm2.Name)
                                    Try
                                        failname = itm2.Name
                                        itm2.Delete()

                                    Catch ex As Exception

                                        Console.WriteLine("Failed to delete " & itm.Name & ": " & failname & " because " & ex.Message)
                                    End Try
                                Next
                            End If

                            If itm.Name.ToLower = "glcache" Then
                                For Each itm3 As DirectoryInfo In itm.GetDirectories

                                    Dim failname = itm3.Name

                                    Try
                                        Console.WriteLine("Deleting GLShader : " & itm3.Name)
                                        itm3.Delete(True)

                                    Catch ex As Exception
                                        Console.WriteLine("Failed to delete " & itm.Name & ": " & failname & " because " & ex.Message)
                                    End Try
                                Next

                            End If
                        Next
                    End If

                End If

            End If

            Passes -= 1

            Console.WriteLine("Passes " & maxPasses - Passes & " of " & maxPasses)
            Threading.Thread.Sleep(sleeptime)
        End While

        Console.WriteLine()

        If nvpath Is Nothing Then
            Console.WriteLine("Nvidia Driver Control Program Not installed")
        Else
            Console.WriteLine("Nvidia Driver Control program installed on """ & nvpath & """")
            Console.WriteLine()

            If popshaderprog Then
                Console.WriteLine("Go to settings in NVidia Driver control app and Graphics -> Global settings -> Shader Cash size-> Disabled")
                Console.WriteLine()
                Console.WriteLine("Be sure to set it back to your default value")

                Try
                    Process.Start(nvpath)
                Catch ex As Exception

                End Try

            End If


        End If



        Console.WriteLine()
        Console.WriteLine("Press any key to close")
        Console.ReadKey()

    End Sub
End Module
