Public Class AdminSeguridad


    '/// Encripta una cadena
    Public Shared Function Encriptar(ByVal _cadenaAencriptar As String) As String

        Dim result As String = String.Empty

        If Not String.IsNullOrEmpty(_cadenaAencriptar) Then
            Dim encryted As Byte() = System.Text.Encoding.Unicode.GetBytes(_cadenaAencriptar)
            result = Convert.ToBase64String(encryted)
        End If

        Return result

    End Function
    '/// Esta función desencripta la cadena que le envíamos en el parámentro de entrada.
    Public Shared Function DesEncriptar(ByVal _cadenaAdesencriptar As String) As String
        Dim result As String = String.Empty

        If Not String.IsNullOrEmpty(_cadenaAdesencriptar) Then
            Dim decryted As Byte() = Convert.FromBase64String(_cadenaAdesencriptar)
            '//result = System.Text.Encoding.Unicode.GetString(decryted, 0, decryted.ToArray().Length) 
            result = System.Text.Encoding.Unicode.GetString(decryted)
        End If

        Return result

    End Function


End Class
