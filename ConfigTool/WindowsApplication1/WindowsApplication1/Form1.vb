Imports System.IO


Public Class Form1

    Private Function GetAllComputers(XMLds As DataSet)
        Dim myXMLdt As New DataTable

        For Each myXMLdt In XMLds.Tables
            ListBox1.Items.Add(myXMLdt.ToString)
        Next

    End Function

    Private Sub ÖffnenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ÖffnenToolStripMenuItem.Click
        If OpenFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            Dim myXMLds As New DataSet

            ListBox1.Items.Clear()
            myXMLds.ReadXml(OpenFileDialog1.FileName)

            GetAllComputers(myXMLds)

        End If
    End Sub

End Class
