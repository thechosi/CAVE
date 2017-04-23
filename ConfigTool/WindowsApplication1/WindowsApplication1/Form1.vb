Imports System.IO

Public Class Form1

    Private Function GetAllComputers(XMLFile As Xml.XmlDataDocument)

        Dim XMLAttributes As Xml.XmlAttributeCollection
        Dim i As Integer
        Dim j As Integer
        Dim XMLnode As Xml.XmlNodeList

        XMLnode = XMLFile.GetElementsByTagName("master")
        For i = 0 To XMLnode.Count - 1
            XMLAttributes = XMLnode(i).Attributes
            If XMLAttributes IsNot Nothing Then
                For j = 0 To XMLAttributes.Count - 1
                    XMLAttributes.Item(j).InnerText.Trim()
                    If XMLAttributes.Item(j).Name.Equals("name") Then
                        ListBox1.Items.Add(XMLAttributes.Item(j).InnerText.Trim() & " (Master)")
                    End If
                Next
            End If
        Next

        XMLnode = XMLFile.GetElementsByTagName("slave")
        For i = 0 To XMLnode.Count - 1
            XMLAttributes = XMLnode(i).Attributes
            If XMLAttributes IsNot Nothing Then
                For j = 0 To XMLAttributes.Count - 1
                    XMLAttributes.Item(j).InnerText.Trim()
                    If XMLAttributes.Item(j).Name.Equals("name") Then
                        ListBox1.Items.Add(XMLAttributes.Item(j).InnerText.Trim() & " (Slave)")
                    End If
                Next
            End If
        Next
    End Function


    Private Function GetComputerInfo(computerName As String)
        Dim fs As New FileStream(OpenFileDialog1.FileName, FileMode.Open, FileAccess.Read)
        Dim xmlData As String
        Dim XMLFile As New Xml.XmlDataDocument()
        Dim XMLAttributes As Xml.XmlAttributeCollection
        Dim status As String
        Dim i As Integer
        Dim j As Integer
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
                            Me.txt_computerName.Text = XMLAttributes.Item(j).InnerText.Trim()
                            If XMLAttributes.Item(j).InnerText.Trim().Equals(computerName) Then
                                exitFor = False
                                Exit For
                            End If
                        End If
                    End If
                Next
            End If
            If XMLnode(i).HasChildNodes Then
                For j = 0 To XMLnode(i).ChildNodes.Count - 1
                    If XMLnode(i).ChildNodes.Item(j).Name.Equals("relationToOrignal") Then
                        XMLAttributes = XMLnode(i).ChildNodes.Item(j).Attributes
                        If XMLAttributes IsNot Nothing Then

                        End If
                    End If
                    If XMLnode(i).ChildNodes.Item(j).Name.Equals("camera") Then
                        XMLAttributes = XMLnode(i).ChildNodes.Item(j).Attributes
                        If XMLAttributes IsNot Nothing Then
                            If XMLAttributes.Item(0).Name.Equals("eye") Then
                                Me.combo_eye.Items.Add(XMLAttributes.Item(0).InnerText.Trim())
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


    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        Me.GroupBox2.Text = Me.ListBox1.SelectedItem.ToString
        GetComputerInfo(Me.ListBox1.SelectedItem.ToString)

    End Sub

    Private Sub ÖffnenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ÖffnenToolStripMenuItem.Click
        If OpenFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            Dim fs As New FileStream(OpenFileDialog1.FileName, FileMode.Open, FileAccess.Read)
            Dim xmlData As String
            Dim XMLFile As New Xml.XmlDataDocument()

            XMLFile.Load(fs)

            ListBox1.Items.Clear()
            GetAllComputers(XMLFile)

            fs.Close()

        End If
    End Sub

End Class
