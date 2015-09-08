function visibilidad(control, estado) {

    if (estado) {       /*Significa que se oculta*/
        control.style.display = 'block';
    } else {            /*Significa que se visualiza*/
        control.style.display = 'none';
    }
}

function cerrarVentana() {
    window.close();
}

function fc_redimencionarArray(indice) {

    var arreglo = new Array(indice);
    for (i = 0; i < indice; i++) {
        arreglo[i] = new Array(2);
    }
  
    return arreglo;
}

function fc_parametrosData(arreglo) {
    var cadena = "{";
    var total = arreglo.length;
    if (arreglo.length == 0) {
        cadena = cadena + "}";
    } else {
        for (var i = 0; i < arreglo.length; i++) {
            cadena = cadena + "'" + arreglo[i][0] + "'";
            cadena = cadena + ": ";
            cadena = cadena + "'" + arreglo[i][1] + "'";
            if (total - 1 != i) {
                cadena = cadena + ",";
            }
        }
        cadena = cadena + "}";
    }

    return cadena;
}

function fc_cadenaContenidoGrilla(filas) {

    var cadena = '';
    var linea = '';
    for (var i = 0; i < filas.length; i++) {
        linea = filas[i].MatriculaUsuario;
        linea = linea + ";" + filas[i].NombreUsuario;
        linea = linea + ";" + filas[i].Correo;
        if (i != 0) cadena = cadena + "|"
        cadena = cadena + linea;
    }
    return cadena;
}

function fc_IsNullOrEmpty(control) {
    if (control.value == null) {
        return true;
    }
    if (control.value == "") {
        return true;
    }
    return false;
}


function eventoSoloNumeros() {
    // NOTE: Backspace = 8, Enter = 13, '0' = 48, '9' = 57		
    var key = event.keyCode;
    if ((key <= 13 || (key >= 48 && key <= 57) || (key >= 96 && key <= 105) || (key == 33) || (key == 34) || (key == 35) || (key == 36) || (key == 37) || (key == 38) || (key == 39) || (key == 40) || (key == 45) || (key == 46) || (key == 86) || (key == 67) || (key == 88)) == true)
        event.returnValue = true;
    else
        event.returnValue = false;
}

function eventoAlfaNumerico(event) {
    if ((event.keyCode >= 65 && event.keyCode <= 90)) {
        event.returnValue = true;
    }
    else {
        eventoSoloNumeros(event);
    }
}

function soloLectura(evt) {
    var nav4 = window.Event ? true : false;
    var key = nav4 ? evt.which : evt.keyCode;
    return false;
}

function fc_getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}

function fc_left(str, n) {
    if (n <= 0)
        return "";
    else if (n > String(str).length)
        return str;
    else
        return String(str).substring(0, n);
}
function fc_right(str, n) {
    if (n <= 0)
        return "";
    else if (n > String(str).length)
        return str;
    else {
        var iLen = String(str).length;
        return String(str).substring(iLen, iLen - n);
    }
}
function ValidNumAndDot(e) {
    var tecla = document.all ? tecla = e.keyCode : tecla = e.which;
    //dígito o delete o backspace o 
    return ((tecla > 47 && tecla < 58) || tecla == 46 || tecla == 8 || tecla == 0);
}

function ValoresMonetarios(e) {

            var key;
            if (window.event) // IE
            {
                key = e.keyCode;
            }
            else if (e.which) // Netscape/Firefox/Opera
            {
                key = e.which;
            }

            if (key < 48 || key > 57) { // caracteres no numéricos
                if (key != 44 && key != 46){//Ascii de , y .
                 return false; }
                else
                { return true; }
            }

            return true;
        }
        function solonumeros(e) {

            var key;
            if (window.event) // IE
            {
                key = e.keyCode;
            }
            else if (e.which) // Netscape/Firefox/Opera
            {
                key = e.which;
            }

            //if (key < 48 || key > 57) {
            if (key < 96 || key > 105) {
                if (key != 8 && key != 110) {
                    return false;
                 }                
            }

            return true;
        }


    function ObtenerSubCadena(valorCadena , indexInicio , longitud)
    {
        var varCadena = '';
        varCadena = valorCadena.substring(indexInicio, longitud);
       
        return varCadena;
    }
    function ExtraerNombreArchivo(NombreArchivo) {

        var cadena = '';
        var pos = -1;
        if (NombreArchivo.length > 0) {
            pos = NombreArchivo.lastIndexOf('.')
        }

        if (pos != -1) {
            cadena = NombreArchivo.substring(0, pos);
        }
        
        return cadena;
    }