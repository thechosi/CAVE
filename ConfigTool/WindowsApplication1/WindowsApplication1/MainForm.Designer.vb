<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.grp_computerList = New System.Windows.Forms.GroupBox()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.DateiToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ÖffnenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BeendenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ÜberDasToolToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.grp_computerInfo = New System.Windows.Forms.GroupBox()
        Me.lbl_computerName = New System.Windows.Forms.Label()
        Me.txt_computerName = New System.Windows.Forms.TextBox()
        Me.grp_cam = New System.Windows.Forms.GroupBox()
        Me.combo_eye = New System.Windows.Forms.ComboBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.txt_cam_z = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txt_cam_y = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.txt_cam_x = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.list_cam = New System.Windows.Forms.ListBox()
        Me.grp_relation = New System.Windows.Forms.GroupBox()
        Me.txt_relat_z = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txt_relat_y = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txt_relat_x = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.list_rela = New System.Windows.Forms.ListBox()
        Me.grp_depl = New System.Windows.Forms.GroupBox()
        Me.txt_info = New System.Windows.Forms.TextBox()
        Me.chk_master = New System.Windows.Forms.CheckBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.grp_splane = New System.Windows.Forms.GroupBox()
        Me.txt_splane_z = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txt_splane_y = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txt_splane_x = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.list_splane = New System.Windows.Forms.ListBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txt_port = New System.Windows.Forms.TextBox()
        Me.txt_ipAddress = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btn_configSave = New System.Windows.Forms.Button()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.btn_deployProject = New System.Windows.Forms.Button()
        Me.btn_startProject = New System.Windows.Forms.Button()
        Me.lbl_projectname = New System.Windows.Forms.Label()
        Me.txt_projectname = New System.Windows.Forms.TextBox()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.btn_update = New System.Windows.Forms.Button()
        Me.grp_computerList.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.grp_computerInfo.SuspendLayout()
        Me.grp_cam.SuspendLayout()
        Me.grp_relation.SuspendLayout()
        Me.grp_depl.SuspendLayout()
        Me.grp_splane.SuspendLayout()
        Me.SuspendLayout()
        '
        'ListBox1
        '
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.Location = New System.Drawing.Point(6, 19)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(133, 264)
        Me.ListBox1.TabIndex = 0
        '
        'grp_computerList
        '
        Me.grp_computerList.Controls.Add(Me.ListBox1)
        Me.grp_computerList.Enabled = False
        Me.grp_computerList.Location = New System.Drawing.Point(12, 61)
        Me.grp_computerList.Name = "grp_computerList"
        Me.grp_computerList.Size = New System.Drawing.Size(145, 289)
        Me.grp_computerList.TabIndex = 1
        Me.grp_computerList.TabStop = False
        Me.grp_computerList.Text = "Which Computer"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DateiToolStripMenuItem, Me.ToolStripMenuItem1})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(664, 24)
        Me.MenuStrip1.TabIndex = 2
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'DateiToolStripMenuItem
        '
        Me.DateiToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ÖffnenToolStripMenuItem, Me.BeendenToolStripMenuItem})
        Me.DateiToolStripMenuItem.Name = "DateiToolStripMenuItem"
        Me.DateiToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.DateiToolStripMenuItem.Text = "File"
        '
        'ÖffnenToolStripMenuItem
        '
        Me.ÖffnenToolStripMenuItem.Name = "ÖffnenToolStripMenuItem"
        Me.ÖffnenToolStripMenuItem.Size = New System.Drawing.Size(103, 22)
        Me.ÖffnenToolStripMenuItem.Text = "Open"
        '
        'BeendenToolStripMenuItem
        '
        Me.BeendenToolStripMenuItem.Name = "BeendenToolStripMenuItem"
        Me.BeendenToolStripMenuItem.Size = New System.Drawing.Size(103, 22)
        Me.BeendenToolStripMenuItem.Text = "Close"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ÜberDasToolToolStripMenuItem})
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(24, 20)
        Me.ToolStripMenuItem1.Text = "?"
        '
        'ÜberDasToolToolStripMenuItem
        '
        Me.ÜberDasToolToolStripMenuItem.Name = "ÜberDasToolToolStripMenuItem"
        Me.ÜberDasToolToolStripMenuItem.Size = New System.Drawing.Size(116, 22)
        Me.ÜberDasToolToolStripMenuItem.Text = "About..."
        '
        'grp_computerInfo
        '
        Me.grp_computerInfo.Controls.Add(Me.lbl_computerName)
        Me.grp_computerInfo.Controls.Add(Me.txt_computerName)
        Me.grp_computerInfo.Controls.Add(Me.grp_cam)
        Me.grp_computerInfo.Controls.Add(Me.grp_relation)
        Me.grp_computerInfo.Controls.Add(Me.grp_depl)
        Me.grp_computerInfo.Controls.Add(Me.chk_master)
        Me.grp_computerInfo.Controls.Add(Me.Label6)
        Me.grp_computerInfo.Controls.Add(Me.grp_splane)
        Me.grp_computerInfo.Controls.Add(Me.Label2)
        Me.grp_computerInfo.Controls.Add(Me.txt_port)
        Me.grp_computerInfo.Controls.Add(Me.txt_ipAddress)
        Me.grp_computerInfo.Controls.Add(Me.Label1)
        Me.grp_computerInfo.Enabled = False
        Me.grp_computerInfo.Location = New System.Drawing.Point(163, 61)
        Me.grp_computerInfo.Name = "grp_computerInfo"
        Me.grp_computerInfo.Size = New System.Drawing.Size(489, 289)
        Me.grp_computerInfo.TabIndex = 3
        Me.grp_computerInfo.TabStop = False
        '
        'lbl_computerName
        '
        Me.lbl_computerName.AutoSize = True
        Me.lbl_computerName.Location = New System.Drawing.Point(250, 23)
        Me.lbl_computerName.Name = "lbl_computerName"
        Me.lbl_computerName.Size = New System.Drawing.Size(38, 13)
        Me.lbl_computerName.TabIndex = 13
        Me.lbl_computerName.Text = "Name:"
        '
        'txt_computerName
        '
        Me.txt_computerName.Enabled = False
        Me.txt_computerName.Location = New System.Drawing.Point(294, 20)
        Me.txt_computerName.Name = "txt_computerName"
        Me.txt_computerName.Size = New System.Drawing.Size(114, 20)
        Me.txt_computerName.TabIndex = 12
        '
        'grp_cam
        '
        Me.grp_cam.Controls.Add(Me.combo_eye)
        Me.grp_cam.Controls.Add(Me.Label16)
        Me.grp_cam.Controls.Add(Me.txt_cam_z)
        Me.grp_cam.Controls.Add(Me.Label13)
        Me.grp_cam.Controls.Add(Me.txt_cam_y)
        Me.grp_cam.Controls.Add(Me.Label14)
        Me.grp_cam.Controls.Add(Me.txt_cam_x)
        Me.grp_cam.Controls.Add(Me.Label15)
        Me.grp_cam.Controls.Add(Me.list_cam)
        Me.grp_cam.Enabled = False
        Me.grp_cam.Location = New System.Drawing.Point(6, 162)
        Me.grp_cam.Name = "grp_cam"
        Me.grp_cam.Size = New System.Drawing.Size(231, 121)
        Me.grp_cam.TabIndex = 5
        Me.grp_cam.TabStop = False
        Me.grp_cam.Text = "Camera"
        '
        'combo_eye
        '
        Me.combo_eye.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.combo_eye.FormattingEnabled = True
        Me.combo_eye.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.combo_eye.Items.AddRange(New Object() {"left", "right"})
        Me.combo_eye.Location = New System.Drawing.Point(47, 16)
        Me.combo_eye.Name = "combo_eye"
        Me.combo_eye.Size = New System.Drawing.Size(178, 21)
        Me.combo_eye.TabIndex = 8
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(6, 20)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(28, 13)
        Me.Label16.TabIndex = 7
        Me.Label16.Text = "Eye:"
        '
        'txt_cam_z
        '
        Me.txt_cam_z.Location = New System.Drawing.Point(136, 95)
        Me.txt_cam_z.Name = "txt_cam_z"
        Me.txt_cam_z.Size = New System.Drawing.Size(89, 20)
        Me.txt_cam_z.TabIndex = 6
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(82, 98)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(48, 13)
        Me.Label13.TabIndex = 5
        Me.Label13.Text = "z-Achse:"
        '
        'txt_cam_y
        '
        Me.txt_cam_y.Location = New System.Drawing.Point(136, 69)
        Me.txt_cam_y.Name = "txt_cam_y"
        Me.txt_cam_y.Size = New System.Drawing.Size(89, 20)
        Me.txt_cam_y.TabIndex = 4
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(82, 72)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(48, 13)
        Me.Label14.TabIndex = 3
        Me.Label14.Text = "y-Achse:"
        '
        'txt_cam_x
        '
        Me.txt_cam_x.Location = New System.Drawing.Point(136, 43)
        Me.txt_cam_x.Name = "txt_cam_x"
        Me.txt_cam_x.Size = New System.Drawing.Size(89, 20)
        Me.txt_cam_x.TabIndex = 2
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(82, 46)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(48, 13)
        Me.Label15.TabIndex = 1
        Me.Label15.Text = "x-Achse:"
        '
        'list_cam
        '
        Me.list_cam.FormattingEnabled = True
        Me.list_cam.Items.AddRange(New Object() {"rotation"})
        Me.list_cam.Location = New System.Drawing.Point(7, 46)
        Me.list_cam.Name = "list_cam"
        Me.list_cam.Size = New System.Drawing.Size(68, 69)
        Me.list_cam.TabIndex = 0
        '
        'grp_relation
        '
        Me.grp_relation.Controls.Add(Me.txt_relat_z)
        Me.grp_relation.Controls.Add(Me.Label10)
        Me.grp_relation.Controls.Add(Me.txt_relat_y)
        Me.grp_relation.Controls.Add(Me.Label11)
        Me.grp_relation.Controls.Add(Me.txt_relat_x)
        Me.grp_relation.Controls.Add(Me.Label12)
        Me.grp_relation.Controls.Add(Me.list_rela)
        Me.grp_relation.Enabled = False
        Me.grp_relation.Location = New System.Drawing.Point(6, 46)
        Me.grp_relation.Name = "grp_relation"
        Me.grp_relation.Size = New System.Drawing.Size(231, 110)
        Me.grp_relation.TabIndex = 7
        Me.grp_relation.TabStop = False
        Me.grp_relation.Text = "Origin"
        '
        'txt_relat_z
        '
        Me.txt_relat_z.Location = New System.Drawing.Point(136, 73)
        Me.txt_relat_z.Name = "txt_relat_z"
        Me.txt_relat_z.Size = New System.Drawing.Size(89, 20)
        Me.txt_relat_z.TabIndex = 6
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(82, 76)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(48, 13)
        Me.Label10.TabIndex = 5
        Me.Label10.Text = "z-Achse:"
        '
        'txt_relat_y
        '
        Me.txt_relat_y.Location = New System.Drawing.Point(136, 47)
        Me.txt_relat_y.Name = "txt_relat_y"
        Me.txt_relat_y.Size = New System.Drawing.Size(89, 20)
        Me.txt_relat_y.TabIndex = 4
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(82, 50)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(48, 13)
        Me.Label11.TabIndex = 3
        Me.Label11.Text = "y-Achse:"
        '
        'txt_relat_x
        '
        Me.txt_relat_x.Location = New System.Drawing.Point(136, 21)
        Me.txt_relat_x.Name = "txt_relat_x"
        Me.txt_relat_x.Size = New System.Drawing.Size(89, 20)
        Me.txt_relat_x.TabIndex = 2
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(82, 24)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(48, 13)
        Me.Label12.TabIndex = 1
        Me.Label12.Text = "x-Achse:"
        '
        'list_rela
        '
        Me.list_rela.FormattingEnabled = True
        Me.list_rela.Items.AddRange(New Object() {"position"})
        Me.list_rela.Location = New System.Drawing.Point(7, 20)
        Me.list_rela.Name = "list_rela"
        Me.list_rela.Size = New System.Drawing.Size(68, 82)
        Me.list_rela.TabIndex = 0
        '
        'grp_depl
        '
        Me.grp_depl.Controls.Add(Me.txt_info)
        Me.grp_depl.Location = New System.Drawing.Point(243, 162)
        Me.grp_depl.Name = "grp_depl"
        Me.grp_depl.Size = New System.Drawing.Size(240, 121)
        Me.grp_depl.TabIndex = 11
        Me.grp_depl.TabStop = False
        Me.grp_depl.Text = "Information"
        '
        'txt_info
        '
        Me.txt_info.Location = New System.Drawing.Point(10, 16)
        Me.txt_info.Multiline = True
        Me.txt_info.Name = "txt_info"
        Me.txt_info.ReadOnly = True
        Me.txt_info.Size = New System.Drawing.Size(224, 99)
        Me.txt_info.TabIndex = 0
        '
        'chk_master
        '
        Me.chk_master.AutoSize = True
        Me.chk_master.Enabled = False
        Me.chk_master.Location = New System.Drawing.Point(462, 23)
        Me.chk_master.Name = "chk_master"
        Me.chk_master.Size = New System.Drawing.Size(15, 14)
        Me.chk_master.TabIndex = 6
        Me.chk_master.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(414, 23)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(42, 13)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "Master:"
        '
        'grp_splane
        '
        Me.grp_splane.Controls.Add(Me.txt_splane_z)
        Me.grp_splane.Controls.Add(Me.Label5)
        Me.grp_splane.Controls.Add(Me.txt_splane_y)
        Me.grp_splane.Controls.Add(Me.Label4)
        Me.grp_splane.Controls.Add(Me.txt_splane_x)
        Me.grp_splane.Controls.Add(Me.Label3)
        Me.grp_splane.Controls.Add(Me.list_splane)
        Me.grp_splane.Enabled = False
        Me.grp_splane.Location = New System.Drawing.Point(243, 46)
        Me.grp_splane.Name = "grp_splane"
        Me.grp_splane.Size = New System.Drawing.Size(240, 110)
        Me.grp_splane.TabIndex = 4
        Me.grp_splane.TabStop = False
        Me.grp_splane.Text = "ScreenPlane"
        '
        'txt_splane_z
        '
        Me.txt_splane_z.Location = New System.Drawing.Point(136, 73)
        Me.txt_splane_z.Name = "txt_splane_z"
        Me.txt_splane_z.Size = New System.Drawing.Size(98, 20)
        Me.txt_splane_z.TabIndex = 6
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(82, 76)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(48, 13)
        Me.Label5.TabIndex = 5
        Me.Label5.Text = "z-Achse:"
        '
        'txt_splane_y
        '
        Me.txt_splane_y.Location = New System.Drawing.Point(136, 47)
        Me.txt_splane_y.Name = "txt_splane_y"
        Me.txt_splane_y.Size = New System.Drawing.Size(98, 20)
        Me.txt_splane_y.TabIndex = 4
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(82, 50)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(48, 13)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "y-Achse:"
        '
        'txt_splane_x
        '
        Me.txt_splane_x.Location = New System.Drawing.Point(136, 21)
        Me.txt_splane_x.Name = "txt_splane_x"
        Me.txt_splane_x.Size = New System.Drawing.Size(98, 20)
        Me.txt_splane_x.TabIndex = 2
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(82, 24)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(48, 13)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "x-Achse:"
        '
        'list_splane
        '
        Me.list_splane.FormattingEnabled = True
        Me.list_splane.Items.AddRange(New Object() {"position", "rotation", "scale"})
        Me.list_splane.Location = New System.Drawing.Point(7, 20)
        Me.list_splane.Name = "list_splane"
        Me.list_splane.Size = New System.Drawing.Size(68, 82)
        Me.list_splane.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(166, 23)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(29, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Port:"
        '
        'txt_port
        '
        Me.txt_port.Location = New System.Drawing.Point(201, 20)
        Me.txt_port.Name = "txt_port"
        Me.txt_port.Size = New System.Drawing.Size(43, 20)
        Me.txt_port.TabIndex = 2
        '
        'txt_ipAddress
        '
        Me.txt_ipAddress.Location = New System.Drawing.Point(67, 20)
        Me.txt_ipAddress.Name = "txt_ipAddress"
        Me.txt_ipAddress.Size = New System.Drawing.Size(93, 20)
        Me.txt_ipAddress.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 23)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(55, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "IP-Adress:"
        '
        'btn_configSave
        '
        Me.btn_configSave.Enabled = False
        Me.btn_configSave.Location = New System.Drawing.Point(526, 356)
        Me.btn_configSave.Name = "btn_configSave"
        Me.btn_configSave.Size = New System.Drawing.Size(126, 23)
        Me.btn_configSave.TabIndex = 4
        Me.btn_configSave.Text = "&Save Configuration"
        Me.btn_configSave.UseVisualStyleBackColor = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'SaveFileDialog1
        '
        Me.SaveFileDialog1.FileName = "node-config.xml"
        Me.SaveFileDialog1.Filter = "XML files|*.xml"
        '
        'btn_deployProject
        '
        Me.btn_deployProject.Enabled = False
        Me.btn_deployProject.Location = New System.Drawing.Point(469, 34)
        Me.btn_deployProject.Name = "btn_deployProject"
        Me.btn_deployProject.Size = New System.Drawing.Size(57, 23)
        Me.btn_deployProject.TabIndex = 5
        Me.btn_deployProject.Text = "&Deploy"
        Me.btn_deployProject.UseVisualStyleBackColor = True
        '
        'btn_startProject
        '
        Me.btn_startProject.Enabled = False
        Me.btn_startProject.Location = New System.Drawing.Point(595, 34)
        Me.btn_startProject.Name = "btn_startProject"
        Me.btn_startProject.Size = New System.Drawing.Size(57, 23)
        Me.btn_startProject.TabIndex = 6
        Me.btn_startProject.Text = "&Start"
        Me.btn_startProject.UseVisualStyleBackColor = True
        '
        'lbl_projectname
        '
        Me.lbl_projectname.AutoSize = True
        Me.lbl_projectname.Location = New System.Drawing.Point(15, 37)
        Me.lbl_projectname.Name = "lbl_projectname"
        Me.lbl_projectname.Size = New System.Drawing.Size(86, 13)
        Me.lbl_projectname.TabIndex = 7
        Me.lbl_projectname.Text = "Name of Project:"
        '
        'txt_projectname
        '
        Me.txt_projectname.Location = New System.Drawing.Point(107, 34)
        Me.txt_projectname.Name = "txt_projectname"
        Me.txt_projectname.ReadOnly = True
        Me.txt_projectname.Size = New System.Drawing.Size(356, 20)
        Me.txt_projectname.TabIndex = 8
        '
        'btn_update
        '
        Me.btn_update.Enabled = False
        Me.btn_update.Location = New System.Drawing.Point(532, 34)
        Me.btn_update.Name = "btn_update"
        Me.btn_update.Size = New System.Drawing.Size(57, 23)
        Me.btn_update.TabIndex = 9
        Me.btn_update.Text = "&Update"
        Me.btn_update.UseVisualStyleBackColor = True
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(664, 387)
        Me.Controls.Add(Me.btn_update)
        Me.Controls.Add(Me.txt_projectname)
        Me.Controls.Add(Me.lbl_projectname)
        Me.Controls.Add(Me.btn_startProject)
        Me.Controls.Add(Me.btn_deployProject)
        Me.Controls.Add(Me.btn_configSave)
        Me.Controls.Add(Me.grp_computerInfo)
        Me.Controls.Add(Me.grp_computerList)
        Me.Controls.Add(Me.MenuStrip1)
        Me.HelpButton = True
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "MainForm"
        Me.Text = "CaveUnity-ConfigTool"
        Me.grp_computerList.ResumeLayout(False)
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.grp_computerInfo.ResumeLayout(False)
        Me.grp_computerInfo.PerformLayout()
        Me.grp_cam.ResumeLayout(False)
        Me.grp_cam.PerformLayout()
        Me.grp_relation.ResumeLayout(False)
        Me.grp_relation.PerformLayout()
        Me.grp_depl.ResumeLayout(False)
        Me.grp_depl.PerformLayout()
        Me.grp_splane.ResumeLayout(False)
        Me.grp_splane.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ListBox1 As ListBox
    Friend WithEvents grp_computerList As GroupBox
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents DateiToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents BeendenToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents ÜberDasToolToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents grp_computerInfo As GroupBox
    Friend WithEvents Label2 As Label
    Friend WithEvents txt_port As TextBox
    Friend WithEvents txt_ipAddress As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents grp_splane As GroupBox
    Friend WithEvents txt_splane_z As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents txt_splane_y As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents txt_splane_x As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents list_splane As ListBox
    Friend WithEvents chk_master As CheckBox
    Friend WithEvents Label6 As Label
    Friend WithEvents btn_configSave As Button
    Friend WithEvents grp_depl As GroupBox
    Friend WithEvents grp_cam As GroupBox
    Friend WithEvents txt_cam_z As TextBox
    Friend WithEvents Label13 As Label
    Friend WithEvents txt_cam_y As TextBox
    Friend WithEvents Label14 As Label
    Friend WithEvents txt_cam_x As TextBox
    Friend WithEvents Label15 As Label
    Friend WithEvents list_cam As ListBox
    Friend WithEvents grp_relation As GroupBox
    Friend WithEvents txt_relat_z As TextBox
    Friend WithEvents Label10 As Label
    Friend WithEvents txt_relat_y As TextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents txt_relat_x As TextBox
    Friend WithEvents Label12 As Label
    Friend WithEvents list_rela As ListBox
    Friend WithEvents combo_eye As ComboBox
    Friend WithEvents Label16 As Label
    Friend WithEvents ÖffnenToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
    Friend WithEvents lbl_computerName As Label
    Friend WithEvents txt_computerName As TextBox
    Friend WithEvents SaveFileDialog1 As SaveFileDialog
    Friend WithEvents txt_info As TextBox
    Friend WithEvents btn_deployProject As Button
    Friend WithEvents btn_startProject As Button
    Friend WithEvents lbl_projectname As Label
    Friend WithEvents txt_projectname As TextBox
    Friend WithEvents FolderBrowserDialog1 As FolderBrowserDialog
    Friend WithEvents btn_update As Button
End Class
