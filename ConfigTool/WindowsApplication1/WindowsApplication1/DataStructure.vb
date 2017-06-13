
Public Class Computer
    Public ip As String = ""
    Public screenplane As New ScreenPlane

    'DeployInfos
    Public name As String = ""
    Public username As String = ""


    Public Overridable Function ToString()
        Return ip
    End Function

End Class

Public Class Master : Inherits Computer
    Public origin As New Origin
    Public port As String = ""

    Public Overrides Function ToString()
        Return MyBase.ToString & " (Master)"
    End Function

End Class

Public Class Slave : Inherits Computer
    Public camera As New Camera

    Public Overrides Function ToString()
        Return MyBase.ToString & " (Slave)"
    End Function
End Class

Public Class Origin
    Public position As New Vektor
    Public rotation As New Vektor
End Class

Public Class Vektor
    Public x As String = "0"
    Public y As String = "0"
    Public z As String = "0"
End Class

Public Class ScreenPlane
    Public position As New Vektor
    Public rotation As New Vektor
    Public scale As New Vektor
End Class

Public Class Camera
    Public eye As String = ""
    Public rotation As New Vektor
End Class