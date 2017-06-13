Imports System.IO

Public Class XMLReader

    Private Function ReadVektor(data As XElement)
        Dim Vektor As New Vektor

        Vektor.x = data.Attribute("x").Value.Trim()
        Vektor.y = data.Attribute("y").Value.Trim()
        Vektor.z = data.Attribute("z").Value.Trim()

        Return Vektor

    End Function

    Private Function ReadOrigin(data As XElement)
        Dim origin As New Origin
        For Each child In data.Elements()
            If child.Name.ToString.Equals("position") Then
                origin.position = ReadVektor(child)
            ElseIf child.Name.ToString.Equals("rotation") Then
                origin.rotation = ReadVektor(child)
            End If
        Next
        Return origin
    End Function

    Private Function ReadCamera(data As XElement)
        Dim camera As New Camera

        camera.eye = data.Attribute("eye").Value.Trim()

        For Each child In data.Elements()
            If child.Name.ToString.Equals("rotation") Then
                camera.rotation = ReadVektor(child)
            End If
        Next

        Return camera
    End Function

    Private Function ReadScreenPlane(data As XElement)
        Dim screenPlane As New ScreenPlane
        For Each child In data.Elements()
            If child.Name.ToString.Equals("position") Then
                screenPlane.position = ReadVektor(child)
            ElseIf child.Name.ToString.Equals("rotation") Then
                screenPlane.rotation = ReadVektor(child)
            ElseIf child.Name.ToString.Equals("scale") Then
                screenPlane.scale = ReadVektor(child)
            End If
        Next

        Return screenPlane
    End Function

    Public Function GetConfig(configfilepath As String, category As String)
        Dim fs As New FileStream(configfilepath, FileMode.Open, FileAccess.Read)
        Dim xml As XDocument = XDocument.Load(fs)
        fs.Close()
        Dim value As String = ""

        For Each element In xml.<config>.Elements()
            If element.Name.ToString.Equals(category) Then
                value = element.Value
            End If
        Next
        Return value
    End Function

    Public Function GetAllComputers(path As String)
        Dim computers As New List(Of Computer)

        Dim xml As XDocument = XDocument.Load(path)

        For Each element In xml.<config>.Elements()

            ' MasterPC
            If element.Name.ToString.Equals("master") Then
                Dim computer As New Master

                For Each child In element.Elements()
                    If child.Name.ToString.Equals("origin") Then
                        computer.origin = ReadOrigin(child)
                    End If
                    If child.Name.ToString.Equals("screenplane") Then
                        computer.screenplane = ReadScreenPlane(child)
                    End If
                Next

                computer.port = element.Attribute("port").Value.Trim()
                computer.ip = element.Attribute("ip").Value.Trim()

                computers.Add(computer)

                'SlavePCs
            Else
                Dim computer As New Slave

                For Each child In element.Elements()
                    If child.Name.ToString.Equals("camera") Then
                        computer.camera = ReadCamera(child)
                    End If
                    If child.Name.ToString.Equals("screenplane") Then
                        computer.screenplane = ReadScreenPlane(child)
                    End If
                Next

                computer.ip = element.Attribute("ip").Value.Trim()
                computers.Add(computer)
            End If
        Next

        Return computers
    End Function

End Class
