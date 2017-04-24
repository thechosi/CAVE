Imports System.IO

Public Class XMLWriter

    Private Function WriteVektor(data As XElement, vektor As Vektor)

        data.Attribute("x").Value = Vektor.x
        data.Attribute("y").Value = Vektor.y
        data.Attribute("z").Value = Vektor.z

    End Function

    Private Function WriteRelationToOrigin(data As XElement, computer As Computer)

        For Each child In data.Elements()
            If child.Name.ToString.Equals("position") Then
                WriteVektor(child, DirectCast(computer, Master).relationToOrigin.position)
            ElseIf child.Name.ToString.Equals("rotation") Then
                WriteVektor(child, DirectCast(computer, Master).relationToOrigin.rotation)
            End If
        Next

    End Function

    Private Function WriteCamera(data As XElement, computer As Computer)

        data.Attribute("eye").Value = DirectCast(computer, Slave).camera.eye.Trim()

        For Each child In data.Elements()
            If child.Name.ToString.Equals("rotation") Then
                WriteVektor(child, DirectCast(computer, Slave).camera.rotation)
            End If
        Next
    End Function

    Private Function WriteScreenPlane(data As XElement, computer As Computer)
        Dim screenPlane As New ScreenPlane
        For Each child In data.Elements()
            If child.Name.ToString.Equals("pa") Then
                WriteVektor(child, computer.screenplane.pa)
            ElseIf child.Name.ToString.Equals("pb") Then
                WriteVektor(child, computer.screenplane.pb)
            ElseIf child.Name.ToString.Equals("pc") Then
                WriteVektor(child, computer.screenplane.pc)
            ElseIf child.Name.ToString.Equals("pe") Then
                WriteVektor(child, computer.screenplane.pe)
            End If
        Next

    End Function

    Public Function SetAllComputers(filename As String, saveFileName As String, computers As List(Of Computer))
        Dim openFile As New FileStream(filename, FileMode.Open, FileAccess.Read)
        Dim xml As XDocument = XDocument.Load(openFile)
        Dim counter As Integer = 0
        openFile.Close()
        For Each element In xml.<config>.Elements()
            ' MasterPC
            If element.Name.ToString.Equals("master") Then

                If TypeOf computers.Item(counter) Is Master Then
                    For Each child In element.Elements()
                        If child.Name.ToString.Equals("relationToOrigin") Then
                            WriteRelationToOrigin(child, computers.Item(counter))
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
        Dim saveFile As New FileStream(saveFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite)

        xml.Save(saveFile)
        saveFile.Close()

    End Function

End Class
