Imports System.Data.SqlClient
Imports Ferreyros.Utiles.Estructuras
Imports Ferreyros.DACotizador
Imports Ferreyros.BECotizador
Imports log4net

Public Class bcAprobador
    Private pr_strError As String
    Private _daAprobador As daAprobador = Nothing
    Private Shared ReadOnly log As ILog = LogManager.GetLogger(GetType(bcAprobador))

    Public ReadOnly Property ErrorDes() As String
        Get
            Return pr_strError
        End Get
    End Property

    Public Function Anular(ByVal strConexion As String, ByVal eAprobador As beAprobador) As Boolean
        Dim blnResultado As Boolean
        Dim trnSql As SqlTransaction = Nothing
        Try
            Dim oAprobador As New daAprobador
            Using cnnSql As New SqlConnection(strConexion)
                cnnSql.Open()
                trnSql = cnnSql.BeginTransaction
                If Not oAprobador.Anular(cnnSql, eAprobador, trnSql) Then
                    Throw New Exception(oAprobador.ErrorDes)
                End If
                trnSql.Commit()
            End Using
            blnResultado = True
        Catch ex As Exception
            If trnSql IsNot Nothing Then
                If trnSql.Connection IsNot Nothing Then
                    trnSql.Rollback()
                End If
            End If
            pr_strError = ex.Message.ToString
        End Try
        Return blnResultado
    End Function

    Public Sub ListarAprobador(ByVal eConexion As String, ByVal eAprobador As beAprobador, ByRef eValidacion As beValidacion, ByRef eDetalle As List(Of beAprobador))

        If String.IsNullOrEmpty(eConexion) OrElse eAprobador Is Nothing Then
            Return
        End If

        Try
            _daAprobador = New daAprobador
            _daAprobador.ListarAprobador(eConexion, eAprobador, eValidacion, eDetalle)
        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString
            log.Error(eValidacion.cadenaAleatoria + ": " + eValidacion.mensaje)
        End Try

    End Sub

    Public Sub MantenimientoAprobador(ByVal eConexion As String, ByRef eAprobador As beAprobador, ByRef eDetalle As List(Of beAprobadorUsuario), ByRef eValidacion As beValidacion)

        If String.IsNullOrEmpty(eConexion) Then
            Return
        End If

        Try
            _daAprobador = New daAprobador
            _daAprobador.MantenimientoAprobador(eConexion, eAprobador, eDetalle, eValidacion)
        Catch ex As Exception
            eValidacion.mensaje = ex.Message.ToString
            log.Error(eValidacion.cadenaAleatoria + ": " + eValidacion.mensaje)
        End Try

    End Sub

End Class