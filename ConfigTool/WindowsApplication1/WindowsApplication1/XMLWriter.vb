Imports System.IO

Public Class XMLWriter

    Public Sub SetAllComputers(filename As String, computers As List(Of Computer))
        Dim xml As New XDocument
        Dim rootTree As XElement = <config></config>
        xml.Add(rootTree)

        Dim counter As Integer = 0
        Dim srcTree As XElement
        While counter < computers.Count
            If TypeOf computers.Item(counter) Is Master Then
                srcTree =
                    <master ip=<%= computers.Item(counter).ip %> port=<%= DirectCast(computers.Item(counter), Master).port %>>
                        <origin>
                            <position x=<%= DirectCast(computers.Item(counter), Master).origin.position.x %> y=<%= DirectCast(computers.Item(counter), Master).origin.position.y %> z=<%= DirectCast(computers.Item(counter), Master).origin.position.z %>/>
                            <rotation x=<%= DirectCast(computers.Item(counter), Master).origin.rotation.x %> y=<%= DirectCast(computers.Item(counter), Master).origin.rotation.y %> z=<%= DirectCast(computers.Item(counter), Master).origin.rotation.z %>/>
                        </origin>
                        <screenplane>
                            <position x=<%= computers.Item(counter).screenplane.position.x %> y=<%= computers.Item(counter).screenplane.position.y %> z=<%= computers.Item(counter).screenplane.position.z %>/>
                            <rotation x=<%= computers.Item(counter).screenplane.rotation.x %> y=<%= computers.Item(counter).screenplane.rotation.y %> z=<%= computers.Item(counter).screenplane.rotation.z %>/>
                            <scale x=<%= computers.Item(counter).screenplane.scale.x %> y=<%= computers.Item(counter).screenplane.scale.y %> z=<%= computers.Item(counter).screenplane.scale.z %>/>
                        </screenplane>
                    </master>
            Else
                srcTree =
                    <slave ip=<%= computers.Item(counter).ip %>>
                        <camera eye=<%= DirectCast(computers.Item(counter), Slave).camera.eye %>>
                            <rotation x=<%= DirectCast(computers.Item(counter), Slave).camera.rotation.x %> y=<%= DirectCast(computers.Item(counter), Slave).camera.rotation.y %> z=<%= DirectCast(computers.Item(counter), Slave).camera.rotation.z %>/>
                        </camera>
                        <screenplane>
                            <position x=<%= computers.Item(counter).screenplane.position.x %> y=<%= computers.Item(counter).screenplane.position.y %> z=<%= computers.Item(counter).screenplane.position.z %>/>
                            <rotation x=<%= computers.Item(counter).screenplane.rotation.x %> y=<%= computers.Item(counter).screenplane.rotation.y %> z=<%= computers.Item(counter).screenplane.rotation.z %>/>
                            <scale x=<%= computers.Item(counter).screenplane.scale.x %> y=<%= computers.Item(counter).screenplane.scale.y %> z=<%= computers.Item(counter).screenplane.scale.z %>/>
                        </screenplane>
                    </slave>
            End If
            xml.Root.Add(srcTree)
            counter = counter + 1
        End While

        xml.Save(filename)
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