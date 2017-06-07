Imports System.IO

Public Class XMLWriter

    Private Sub WriteVektor(data As XElement, vektor As Vektor)

        data.Attribute("x").Value = vektor.x
        data.Attribute("y").Value = vektor.y
        data.Attribute("z").Value = vektor.z

    End Sub

    Private Sub WriteOrigin(data As XElement, computer As Computer)

        For Each child In data.Elements()
            If child.Name.ToString.Equals("position") Then
                WriteVektor(child, DirectCast(computer, Master).origin.position)
            End If
        Next

    End Sub

    Private Sub WriteCamera(data As XElement, computer As Computer)

        data.Attribute("eye").Value = DirectCast(computer, Slave).camera.eye.Trim()

        For Each child In data.Elements()
            If child.Name.ToString.Equals("rotation") Then
                WriteVektor(child, DirectCast(computer, Slave).camera.rotation)
            End If
        Next
    End Sub

    Private Sub WriteScreenPlane(data As XElement, computer As Computer)
        Dim screenPlane As New ScreenPlane
        For Each child In data.Elements()
            If child.Name.ToString.Equals("position") Then
                WriteVektor(child, computer.screenplane.position)
            ElseIf child.Name.ToString.Equals("rotation") Then
                WriteVektor(child, computer.screenplane.rotation)
            ElseIf child.Name.ToString.Equals("scale") Then
                WriteVektor(child, computer.screenplane.scale)
            End If
        Next

    End Sub

    Public Sub SetAllComputers(filename As String, computers As List(Of Computer))
        Dim openFile As New FileStream(filename, FileMode.Open, FileAccess.Read)
        Dim xml As XDocument = XDocument.Load(openFile)
        Dim counter As Integer = 0
        openFile.Close()
        For Each element In xml.<config>.Elements()
            ' MasterPC
            If element.Name.ToString.Equals("master") Then

                If TypeOf computers.Item(counter) Is Master Then
                    For Each child In element.Elements()
                        If child.Name.ToString.Equals("origin") Then
                            WriteOrigin(child, computers.Item(counter))
                        End If
                        If child.Name.ToString.Equals("screenplane") Then
                            WriteScreenPlane(child, computers.Item(counter))
                        End If
                    Next
                    element.Attribute("port").Value = DirectCast(computers.Item(counter), Master).port
                    element.Attribute("ip").Value = computers.Item(counter).ip
                End If
                'SlavePCs
            Else
                For Each child In element.Elements()
                    If child.Name.ToString.Equals("camera") Then
                        WriteCamera(child, computers.Item(counter))
                    End If
                    If child.Name.ToString.Equals("screenplane") Then
                        WriteScreenPlane(child, computers.Item(counter))
                    End If
                Next

                element.Attribute("ip").Value = computers.Item(counter).ip
            End If
            counter = counter + 1
        Next
        Dim saveFile As New FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite)

        xml.Save(saveFile)
        saveFile.Close()

    End Sub

    Public Sub SetConfig(configpath As String, category As String, value As String)

        Dim xml As XDocument = XDocument.Load(configpath)

        For Each element In xml.<config>.Elements()
            If element.Name.ToString.Equals(category) Then
                element.Value = value
            End If
        Next

        xml.Save(configpath)
    End Sub

End Class