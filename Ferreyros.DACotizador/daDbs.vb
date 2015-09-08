Imports System.Data.OleDb
Imports Ferreyros.Utiles.Estructuras
Imports Ferreyros.BECotizador
Imports System.Text

Public Class daDbs

    Private blnValido As Boolean
    Private strError As String = String.Empty

    Public ReadOnly Property ErrorDes() As String
        Get
            Return strError
        End Get
    End Property

#Region "Modelo"
    Public Function BuscarModelo(ByVal cnnOledb As OleDbConnection, ByVal Libreria As String, ByVal Codigo As String, _
                                 ByVal Version As String, ByVal Descripcion As String, ByRef dtrModelo As DataRow) As Boolean
        blnValido = False
        strError = String.Empty

        Dim strFiltro As String = String.Format("Select Distinct M.COCD, M.EQMFCD, V.EQMFCOD, M.DS8, V.EQMFPRL, V.EQMFMON, MO.DESCRIPE AS MONEDA " & _
                        "From {0}.EMPMDLH0 M " & _
                        "Inner Join {0}.UMPMDLV0 V On M.COCD = V.EQMFM2 " & _
                        "Left Join {0}.UFPSC060 MO On MO.TABLA='MON' AND MO.CODIGO <>'' And MO.CODIGO=V.EQMFMON " & _
                        "Where M.COCD='{1}' ", Libreria, Codigo)

        If Not Version.Equals(String.Empty) Then
            strFiltro = String.Format("{0} And V.EQMFCOD='{1}'", strFiltro, Version)
        End If

        If Not Descripcion.Equals(String.Empty) Then
            strFiltro = String.Format("{0} And M.DS8='{1}'", strFiltro, Descripcion)
        End If
        Try
            Dim dstModelo As New DataSet
            Dim cmdOledb As OleDbCommand = cnnOledb.CreateCommand
            cmdOledb.CommandText = strFiltro
            cmdOledb.CommandType = CommandType.Text
            cmdOledb.Prepare()
            Dim adpOledb As New OleDbDataAdapter(cmdOledb)
            adpOledb.Fill(dstModelo, Entidad.Modelo.ToString)
            If Not dstModelo.Tables(0).Rows.Count.Equals(0) Then
                dtrModelo = dstModelo.Tables(0).Rows(0)
            End If
            blnValido = True
        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function

    Public Function BuscarModelo(ByVal cnnOledb As OleDbConnection, ByVal Libreria As String, ByVal ValorBusqueda As String, _
                                 ByRef dstModelo As DataSet) As Boolean
        Dim strFiltro As String = String.Format("Select Distinct M.COCD, M.EQMFCD, V.EQMFCOD, M.DS8, V.EQMFPRL, V.EQMFMON From {0}.EMPMDLH0 M, {0}.UMPMDLV0 V Where M.COCD=V.EQMFM2 And M.COCD<>''", Libreria)
        blnValido = False
        strError = String.Empty
        strFiltro = String.Format("{0} And (M.COCD Like '{1}%' Or M.DS8 Like '{1}%') Order By M.COCD", strFiltro, ValorBusqueda)

        Try
            If dstModelo Is Nothing Then
                dstModelo = New DataSet
            End If

            Dim cmdOledb As OleDbCommand = cnnOledb.CreateCommand
            cmdOledb.CommandText = strFiltro
            cmdOledb.CommandType = CommandType.Text
            cmdOledb.Prepare()
            Dim adpOledb As New OleDbDataAdapter(cmdOledb)
            adpOledb.Fill(dstModelo, Entidad.Modelo.ToString)
            blnValido = True
        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function

    Public Function BuscarSoloModelo(ByVal cnnOledb As OleDbConnection, ByVal Libreria As String, ByVal Codigo As String, _
                                 ByRef dstModelo As DataSet) As Boolean
        Dim strFiltro As String = String.Format("Select Distinct M.COCD, M.EQMFCD, M.DS8 From {0}.EMPMDLH0 M, {0}.UMPMDLV0 V Where M.COCD=V.EQMFM2 And M.COCD<>''", Libreria)
        blnValido = False
        strError = String.Empty
        strFiltro = String.Format("{0} And (M.COCD Like '{1}%' Or M.DS8 Like '{1}%') Order By M.COCD", strFiltro, Codigo)

        Try
            If dstModelo Is Nothing Then
                dstModelo = New DataSet
            End If

            Dim cmdOledb As OleDbCommand = cnnOledb.CreateCommand
            cmdOledb.CommandText = strFiltro
            cmdOledb.CommandType = CommandType.Text
            cmdOledb.Prepare()
            Dim adpOledb As New OleDbDataAdapter(cmdOledb)
            adpOledb.Fill(dstModelo, Entidad.Modelo.ToString)
            blnValido = True
        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function
#End Region

#Region "Cliente"
    Public Function BuscarCliente(ByVal cnnOledb As OleDbConnection, ByVal Libreria As String, _
                                ByVal CodigoCliente As String, ByVal Corporacion As String, _
                                ByVal Compañia As String, ByRef dtrCliente As DataRow) As Boolean
        blnValido = False
        strError = String.Empty
        Dim strFiltro As String = String.Format("Select TIPREG,CORP,CIA,CLIE,RSOCIA,DIRECC,LOCALI,TELEFO,INUM,IDOC,TDOC,TNUM,TNUMO From {0}.ucpcb000 Where CLIE='{1}' And CORP='{2}' And CIA='{3}'", Libreria, CodigoCliente, Corporacion, Compañia)
        Try
            Dim dstCliente As New DataSet
            Dim cmdOledb As OleDbCommand = cnnOledb.CreateCommand
            cmdOledb.CommandText = strFiltro
            cmdOledb.CommandType = CommandType.Text
            cmdOledb.Prepare()
            Dim adpOledb As New OleDbDataAdapter(cmdOledb)
            adpOledb.Fill(dstCliente, "Cliente")
            If Not dstCliente.Tables(0).Rows.Count.Equals(0) Then
                dtrCliente = dstCliente.Tables(0).Rows(0)
            End If
            blnValido = True
        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function

    Public Function BuscarCliente(ByVal cnnOledb As OleDbConnection, ByVal Libreria As String, _
                                 ByVal ValorBusqueda As String, ByVal Corporacion As String, _
                                ByVal Compañia As String, ByRef dstCliente As DataSet) As Boolean
        blnValido = False
        strError = String.Empty
        Dim strFiltro As String = String.Format("Select TIPREG,CORP,CIA,CLIE,RSOCIA,DIRECC,LOCALI,TELEFO,INUM,IDOC,TDOC,TNUM,TNUMO From {0}.ucpcb000 Where CLIE like '{1}%' And CORP='{2}' And CIA='{3}'", Libreria, ValorBusqueda, Corporacion, Compañia)
        Try
            If dstCliente Is Nothing Then
                dstCliente = New DataSet
            End If

            Dim cmdOledb As OleDbCommand = cnnOledb.CreateCommand
            cmdOledb.CommandText = strFiltro
            cmdOledb.CommandType = CommandType.Text
            cmdOledb.Prepare()
            Dim adpOledb As New OleDbDataAdapter(cmdOledb)
            adpOledb.Fill(dstCliente, 0, 10, "Codigo")

            adpOledb.SelectCommand.CommandText = String.Format("Select TIPREG,CORP,CIA,CLIE,RSOCIA,DIRECC,LOCALI,TELEFO,INUM,IDOC,TDOC,TNUM,TNUMO From {0}.ucpcb000 Where TNUM like '{1}%' And CORP='{2}' And CIA='{3}'", Libreria, ValorBusqueda, Corporacion, Compañia)
            adpOledb.Fill(dstCliente, 0, 10, "Ruc")

            adpOledb.SelectCommand.CommandText = String.Format("Select TIPREG,CORP,CIA,CLIE,RSOCIA,DIRECC,LOCALI,TELEFO,INUM,IDOC,TDOC,TNUM,TNUMO From {0}.ucpcb000 Where RSOCIA like '%{1}%' And CORP='{2}' And CIA='{3}'", Libreria, ValorBusqueda, Corporacion, Compañia)
            adpOledb.Fill(dstCliente, 0, 10, "Nombre")

            If dstCliente.Tables(1).Rows.Count > 0 Then
                dstCliente.Tables(0).Merge(dstCliente.Tables(1), True)
            End If

            If dstCliente.Tables(2).Rows.Count > 0 Then
                dstCliente.Tables(0).Merge(dstCliente.Tables(2), True)
            End If

            dstCliente.Tables(0).DefaultView.Sort = "CLIE ASC"

            blnValido = True
        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function
#End Region

#Region "Contacto"
    Public Function BuscarContacto(ByVal cnnOledb As OleDbConnection, ByVal Libreria As String, ByVal CodigoContacto As String, _
                                   ByVal CodigoCliente As String, ByRef dtrContacto As DataRow) As Boolean
        blnValido = False
        strError = String.Empty
        Dim strFiltro As String = String.Format("Select CUNO,INFLID,INNM,INNM2 From {0}.SCPINFF0 Where INFLID ='{1}'", Libreria, CodigoContacto)
        If Not CodigoCliente.Equals(String.Empty) Then
            strFiltro = String.Format("{0} And CUNO='{1}'", strFiltro, CodigoCliente)
        End If
        Try
            Dim dstContacto As New DataSet
            Dim cmdOledb As OleDbCommand = cnnOledb.CreateCommand
            cmdOledb.CommandText = strFiltro
            cmdOledb.CommandType = CommandType.Text
            cmdOledb.Prepare()
            Dim adpOledb As New OleDbDataAdapter(cmdOledb)
            adpOledb.Fill(dstContacto, "Contacto")
            If Not dstContacto.Tables(0).Rows.Count.Equals(0) Then
                dtrContacto = dstContacto.Tables(0).Rows(0)
            End If
            blnValido = True
        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function

    Public Function BuscarContactoCliente(ByVal cnnOledb As OleDbConnection, ByVal Libreria As String, ByVal CodigoCliente As String, _
                                 ByVal ValorBusqueda As String, ByRef dstContacto As DataSet) As Boolean
        blnValido = False
        strError = String.Empty
        Dim strFiltro As String = String.Format("Select CUNO,INFLID,INNM,INNM2 From {0}.SCPINFF0 Where CUNO='{1}' And INNM Like '%{2}%'", Libreria, CodigoCliente, ValorBusqueda)
        Try
            If dstContacto Is Nothing Then
                dstContacto = New DataSet
            End If

            Dim cmdOledb As OleDbCommand = cnnOledb.CreateCommand
            cmdOledb.CommandText = strFiltro
            cmdOledb.CommandType = CommandType.Text
            cmdOledb.Prepare()
            Dim adpOledb As New OleDbDataAdapter(cmdOledb)
            adpOledb.Fill(dstContacto, 0, 20, "Contacto")
            dstContacto.Tables(0).DefaultView.Sort = "INNM ASC"
            blnValido = True
        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function
#End Region

#Region "EntidadMaestraDbs"
    Public Function BuscarEntidadMaestraDbs(ByVal cnnOledb As OleDbConnection, ByVal Libreria As String, _
                                            ByVal EntidadMaestraDbs As String, ByRef dstEntidadMaestraDbs As DataSet, _
                                            Optional ByVal Codigo As String = "") As Boolean
        Dim strFiltro As String = String.Format("Select CODIGO, DESCRIPE From {0}.UFPSC060 Where TABLA='{1}' ", Libreria, EntidadMaestraDbs)
        blnValido = False
        strError = String.Empty

        If Codigo.Equals(String.Empty) Then
            strFiltro = String.Format("{0} AND CODIGO<>'' Order By DESCRIPE", strFiltro)
        Else
            strFiltro = String.Format("{0} AND CODIGO='{1}' Order By DESCRIPE", strFiltro, Codigo)
        End If

        Try
            If dstEntidadMaestraDbs Is Nothing Then
                dstEntidadMaestraDbs = New DataSet
            End If

            Dim cmdOledb As OleDbCommand = cnnOledb.CreateCommand
            cmdOledb.CommandText = strFiltro
            cmdOledb.CommandType = CommandType.Text
            cmdOledb.Prepare()
            Dim adpOledb As New OleDbDataAdapter(cmdOledb)
            adpOledb.Fill(dstEntidadMaestraDbs, EntidadMaestraDbs)
            blnValido = True
        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function

    Public Sub ListarEntidadMaestraDbs(ByVal oConexion As String, ByVal oLibreria As String, ByVal oTabla As beTablaMaestra, ByRef oValidacion As beValidacion, ByRef oDetalle As List(Of beTablaMaestra))

        Dim cnnOleDb As OleDbConnection = Nothing
        Dim cmdSql As OleDbCommand = Nothing

        Try
            cnnOleDb = New OleDbConnection(oConexion)
            cnnOleDb.Open()
            cmdSql = cnnOleDb.CreateCommand
            cmdSql.CommandText = Query_EntidadMaestraDbs(oLibreria, oTabla)
            cmdSql.CommandType = CommandType.Text
            Using dr As IDataReader = cmdSql.ExecuteReader
                Dim _CODIGO As Integer = dr.GetOrdinal("CODIGO")
                Dim _DESCRIPE As Integer = dr.GetOrdinal("DESCRIPE")
                Dim Rpt As beTablaMaestra
                While dr.Read
                    Rpt = New beTablaMaestra
                    If Not dr.IsDBNull(_CODIGO) Then Rpt.IdSeccion = dr.GetValue(_CODIGO)
                    If Not dr.IsDBNull(_DESCRIPE) Then Rpt.Descripcion = dr.GetValue(_DESCRIPE)
                    oDetalle.Add(Rpt)
                End While
                dr.Close()
            End Using
            oValidacion.validacion = True
        Catch ex As Exception
            oValidacion.mensaje = ex.Message
        Finally
            If cnnOleDb.State = ConnectionState.Open Then cnnOleDb.Close()
        End Try

    End Sub

    Private Function Query_EntidadMaestraDbs(ByVal Libreria As String, ByVal oTabla As beTablaMaestra) As String

        Dim strFiltro As String = String.Format("Select CODIGO, DESCRIPE From {0}.UFPSC060 Where TABLA='{1}' ", Libreria, oTabla.Nombre)

        If oTabla.IdSeccion.Equals(String.Empty) Then
            strFiltro = String.Format("{0} AND CODIGO<>'' Order By DESCRIPE", strFiltro)
        Else
            strFiltro = String.Format("{0} AND CODIGO='{1}' Order By DESCRIPE", strFiltro, oTabla.IdSeccion)
        End If

        Return strFiltro

    End Function
#End Region

#Region "TipoCambio"
    ''' <summary>
    ''' Obtener Tipo Cambio del día del DBS
    ''' </summary>
    ''' <param name="cnnOledb">Coneccion al origen de datos</param>
    ''' <param name="Libreria">Librería del AS400</param>
    ''' <param name="Corporacion">Código de la corporación</param>
    ''' <param name="Compañia">Código de la compañia</param>
    ''' <param name="Moneda">Código de la moneda</param>
    ''' <param name="Fecha">Fecha a consultar (Formato yyyyMMdd)</param>
    ''' <param name="dtrTipoCambio">Datarow devuelto</param>
    ''' <returns>Datarow</returns>
    ''' <remarks></remarks>
    Public Function ObtenerTipoCambio(ByVal cnnOledb As OleDbConnection, ByVal Libreria As String, _
                                    ByVal Corporacion As String, ByVal Compañia As String, _
                                    ByVal Moneda As String, ByVal Fecha As String, _
                                    ByRef dtrTipoCambio As DataRow) As Boolean
        Dim strFiltro As String
        strError = String.Empty
        blnValido = False

        strFiltro = String.Format("SELECT BANCOM, BANVEN, SMBMON, SMBNAC FROM {0}.UFPTC010 " & _
                 "WHERE CORP='{1}' AND CIA='{2}' AND SMBMON='{3}' AND FECHA='{4}'", Libreria, Corporacion, Compañia, Moneda, Fecha)
        Try
            Dim dstTipoCambio As New DataSet
            Dim cmdOledb As OleDbCommand = cnnOledb.CreateCommand
            cmdOledb.CommandText = strFiltro
            cmdOledb.CommandType = CommandType.Text
            cmdOledb.Prepare()
            Dim adpOledb As New OleDbDataAdapter(cmdOledb)
            adpOledb.Fill(dstTipoCambio, "TipoCambio")
            If Not dstTipoCambio.Tables(0).Rows.Count.Equals(0) Then
                dtrTipoCambio = dstTipoCambio.Tables(0).Rows(0)
            End If
            blnValido = True
        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function

    ''' <summary>
    ''' Obtener Tipo Cambio más reciente del DBS
    ''' </summary>
    ''' <param name="cnnOledb">Coneccion al origen de datos</param>
    ''' <param name="Libreria">Librería del AS400</param>
    ''' <param name="Corporacion">Código de la corporación</param>
    ''' <param name="Compañia">Código de la compañia</param>
    ''' <param name="Moneda">Código de la moneda</param>
    ''' <param name="dtrTipoCambio">Datarow devuelto</param>
    ''' <returns>Datarow</returns>
    ''' <remarks></remarks>
    Public Function ObtenerTipoCambio(ByVal cnnOledb As OleDbConnection, ByVal Libreria As String, _
                                    ByVal Corporacion As String, ByVal Compañia As String, _
                                    ByVal Moneda As String, ByRef dtrTipoCambio As DataRow) As Boolean
        Dim strFiltro As String
        strError = String.Empty
        blnValido = False

        strFiltro = String.Format("SELECT BANCOM, BANVEN, SMBMON, SMBNAC FROM {0}.UFPTC010 " & _
                 "WHERE CORP='{1}' AND CIA='{2}' AND SMBMON='{3}' " & _
                 "Order By FECHA Desc Fetch First 1 Row Only", Libreria, Corporacion, Compañia, Moneda)
        Try
            Dim dstTipoCambio As New DataSet
            Dim cmdOledb As OleDbCommand = cnnOledb.CreateCommand
            cmdOledb.CommandText = strFiltro
            cmdOledb.CommandType = CommandType.Text
            cmdOledb.Prepare()
            Dim adpOledb As New OleDbDataAdapter(cmdOledb)
            adpOledb.Fill(dstTipoCambio, "TipoCambio")
            If Not dstTipoCambio.Tables(0).Rows.Count.Equals(0) Then
                dtrTipoCambio = dstTipoCambio.Tables(0).Rows(0)
            End If
            blnValido = True
        Catch ex As Exception
            strError = ex.Message
        End Try
        Return blnValido
    End Function
#End Region
End Class
