Imports System.IO

Public Class Form1

    Private Function GetAllComputers(fs As FileStream)

        Dim xml As XDocument = XDocument.Load(fs)
        Dim list_ele As String

        For Each element In xml.<config>.Elements()
            If element.Name.ToString.Equals("master") Then
                list_ele = element.Attribute("ip").Value.Trim() & " (Master)"
            Else
                list_ele = element.Attribute("ip").Value.Trim() & " (Slave)"
            End If
            ListBox1.Items.Add(list_ele)
        Next

    End Function

    Private Function GetComputerInfo(computerName As String)
        Dim fs As New FileStream(OpenFileDialog1.FileName, FileMode.Open, FileAccess.Read)
        Dim XMLFile As New Xml.XmlDataDocument()
        Dim XMLAttributes As Xml.XmlAttributeCollection
        Dim status As String
        Dim i As Integer
        Dim j As Integer
        Dim k As Integer
        Dim l As Integer
        Dim m As Integer
        Dim n As Integer
        Dim exitFor As Boolean = True
        Dim XMLnode As Xml.XmlNodeList

        status = computerName.Substring(computerName.IndexOf("(") + 1, computerName.IndexOf(")") - computerName.IndexOf("(") - 1)
        computerName = computerName.Substring(0, computerName.IndexOf("(") - 1)

        XMLFile.Load(fs)

        XMLnode = XMLFile.GetElementsByTagName(status.ToLower())
        For i = 0 To XMLnode.Count - 1
            XMLAttributes = XMLnode(i).Attributes
            If XMLAttributes IsNot Nothing Then
                For j = 0 To XMLAttributes.Count - 1
                    If exitFor Then
                        If XMLAttributes.Item(j).Name.Equals("ip") Then
                            Me.txt_ipAddress.Text = XMLAttributes.Item(j).InnerText.Trim()
                        End If
                        If XMLAttributes.Item(j).Name.Equals("port") Then
                            Me.txt_port.Text = XMLAttributes.Item(j).InnerText.Trim()
                        End If
                        If XMLAttributes.Item(j).Name.Equals("name") Then
                            Me.txt_computerName.Enabled = True
                            Me.txt_computerName.Text = XMLAttributes.Item(j).InnerText.Trim()
                            If XMLAttributes.Item(j).InnerText.Trim().Equals(computerName) Then
                                exitFor = False
                                Exit For
                            End If
                        Else
                            Me.txt_computerName.Enabled = False
                        End If
                    End If
                Next
            End If


            If XMLnode(i).HasChildNodes Then
                For j = 0 To XMLnode(i).ChildNodes.Count - 1
                    If XMLnode(i).ChildNodes.Item(j).Name.Equals("relationToOrigin") Then
                        Me.grp_cam.Enabled = False
                        Me.grp_relation.Enabled = True
                        Me.list_rela.Items.Clear()
                        XMLAttributes = XMLnode(i).ChildNodes.Item(j).Attributes
                        If XMLAttributes IsNot Nothing Then
                            If XMLnode(i).ChildNodes.Item(j).HasChildNodes Then
                                For k = 0 To XMLnode(i).ChildNodes.Item(j).ChildNodes.Count - 1
                                    Me.list_rela.Items.Add(XMLnode(i).ChildNodes.Item(j).ChildNodes.Item(k).Name().Trim())
                                Next
                            End If
                        End If
                    End If
                    If XMLnode(i).ChildNodes.Item(j).Name.Equals("camera") Then
                        Me.grp_cam.Enabled = True
                        Me.grp_relation.Enabled = False
                        Me.combo_eye.Items.Clear()
                        Me.list_rela.Items.Clear()
                        Me.list_cam.Items.Clear()
                        XMLAttributes = XMLnode(i).ChildNodes.Item(j).Attributes
                        If XMLAttributes IsNot Nothing Then
                            If XMLAttributes.Item(0).Name.Equals("eye") Then
                                Me.combo_eye.Items.Add(XMLAttributes.Item(0).InnerText.Trim())
                            End If
                        End If
                    End If
                    If XMLnode(i).ChildNodes.Item(j).Name.Equals("screenplane") Then
                        Me.list_splane.Items.Clear()
                        XMLAttributes = XMLnode(i).ChildNodes.Item(j).Attributes
                        If XMLAttributes IsNot Nothing Then
                            If XMLnode(i).ChildNodes.Item(j).HasChildNodes Then
                                For k = 0 To XMLnode(i).ChildNodes.Item(j).ChildNodes.Count - 1
                                    Me.list_splane.Items.Add(XMLnode(i).ChildNodes.Item(j).ChildNodes.Item(k).Name().Trim())
                                Next
                            End If
                        End If
                    End If
                Next
            End If
        Next

        If status.ToLower().Equals("master") Then
            Me.chk_master.Checked = True
        Else
            Me.chk_master.Checked = False
        End If

        fs.Close()

    End Function


    Private Function GetCamInfo(camName As String)
        Dim fs As New FileStream(OpenFileDialog1.FileName, FileMode.Open, FileAccess.Read)
        Dim xml As XDocument = XDocument.Load(fs)
        Dim computername As String

        computername = Me.ListBox1.SelectedItem.Substring(0, Me.ListBox1.SelectedItem.ToString.IndexOf("(") - 1)

        For Each attribute In xml.<config>.<slave>.Attributes
            If attribute.Name.ToString.Equals("ip") Then
                If attribute.Value.Trim().Equals(computername) Then

                End If
            End If
        Next

        fs.Close()

    End Function

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        Me.GroupBox2.Text = Me.ListBox1.SelectedItem.ToString
        GetComputerInfo(Me.ListBox1.SelectedItem.ToString)

    End Sub

    Private Sub ÖffnenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ÖffnenToolStripMenuItem.Click
        If OpenFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            Dim fs As New FileStream(OpenFileDialog1.FileName, FileMode.Open, FileAccess.Read)
            ListBox1.Items.Clear()
            GetAllComputers(fs)

            fs.Close()

        End If
    End Sub

    Private Sub list_rela_SelectedIndexChanged(sender As Object, e As EventArgs) Handles list_rela.SelectedIndexChanged

    End Sub

    Private Sub combo_eye_SelectedIndexChanged(sender As Object, e As EventArgs) Handles combo_eye.SelectedIndexChanged
        GetCamInfo(Me.combo_eye.SelectedItem.ToString)
    End Sub
End Class
