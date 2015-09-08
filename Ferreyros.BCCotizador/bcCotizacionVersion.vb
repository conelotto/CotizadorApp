Imports System.Data.SqlClient
Imports Ferreyros.BECotizador
Imports Ferreyros.DACotizador

Public Class bcCotizacionVersion

    Private p_strError As String

    Public ReadOnly Property ErrorDes() As String
        Get
            Return p_strError
        End Get
    End Property

    Public Function Insertar(ByVal strConexion As String, ByRef obeCotizacionVersion As beCotizacionVersion) As Boolean
        p_strError = String.Empty
        Dim bolExito As Boolean = False

        Try
            Using CnnSql As New SqlConnection(strConexion)
                CnnSql.Open()
                Dim odaCotizacionVersion As New daCotizacionVersion
                If Not odaCotizacionVersion.Insertar(CnnSql, obeCotizacionVersion) Then
                    Throw New Exception(odaCotizacionVersion.ErrorDes)
                End If
                bolExito = True
            End Using
        Catch ex As Exception
            p_strError = ex.Message
        End Try
        Return bolExito
    End Function
    Public Function BuscarIdCotizacionSap(ByVal strConexion As String, ByVal obeCotizacionVersion As beCotizacionVersion, ByRef Lista As List(Of beCotizacionVersion)) As Boolean
        Dim bolExito As Boolean = False
        'Dim tranSql As SqlTransaction = Nothing
        Try
            Dim odaCotizacionVersion As New daCotizacionVersion
            Using CnnSql As New SqlConnection(strConexion)
                CnnSql.Open()
                If Not odaCotizacionVersion.BuscarIdCotizacionSap(CnnSql, obeCotizacionVersion, Lista) Then
                    Throw New Exception(odaCotizacionVersion.ErrorDes)
                End If
            End Using
            bolExito = True
        Catch ex As Exception
            p_strError = ex.Message.ToString
        End Try
        Return bolExito
    End Function

    Public Function ObtenerNuevaVersion(ByVal strConexion As String, ByVal obeCotizacionVersion As beCotizacionVersion) As beCotizacionVersion

        Dim eCotizacionVersion As New beCotizacionVersion
        Dim NumeroVersion As String = "0"
        Try
            Dim odaCotizacionVersion As New daCotizacionVersion
            Using CnnSql As New SqlConnection(strConexion)
                CnnSql.Open()
                eCotizacionVersion.IdCotizacion = obeCotizacionVersion.IdCotizacion
                eCotizacionVersion = odaCotizacionVersion.CrearVersion(CnnSql, obeCotizacionVersion)
                If eCotizacionVersion Is Nothing Then
                    eCotizacionVersion = obeCotizacionVersion
                    NumeroVersion = obeCotizacionVersion.NumVersion
                    If String.IsNullOrEmpty(NumeroVersion) Then
                        NumeroVersion = "1"
                    Else
                        NumeroVersion = CType((CInt(NumeroVersion) + 1), String)
                    End If
                    eCotizacionVersion.NumVersion = NumeroVersion
                End If
            End Using 

        Catch ex As Exception
            p_strError = ex.Message.ToString
        End Try
        Return eCotizacionVersion
    End Function

    Public Function ActualizarArchivo(ByVal strConexion As String, ByVal obeCotizacionVersion As beCotizacionVersion, ByRef eValidacion As beValidacion) As Boolean
        Dim boolRetorno As Boolean = False 
        Try
            Dim odaCotizacionVersion As New daCotizacionVersion
            Using CnnSql As New SqlConnection(strConexion)
                CnnSql.Open()
                boolRetorno = odaCotizacionVersion.ActualizarArchivo(CnnSql, obeCotizacionVersion, eValidacion)
            End Using

        Catch ex As Exception
            p_strError = ex.Message.ToString
        End Try
        Return boolRetorno
    End Function
    
End Class
