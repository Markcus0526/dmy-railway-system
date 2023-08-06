function isValidEmailAddress(emailAddress) {
    var pattern = new RegExp(/^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i);
    return pattern.test(emailAddress);
}

function getLocalTime(rd) {
    if (rd != null) {
        //var rd2 = new Date(rd.getUTCFullYear(), rd.getUTCMonth(), rd.getUTCDate(), rd.getUTCHours(), rd.getUTCMinutes(), rd.getUTCSeconds());
        var serverOffset = -(8 * 3600000); //UTC(GMT) - ServerTime: (Beijing Time)
        var clientOffset = rd.getTimezoneOffset() * 60000;   //UTC(GMT) - LocalTime

        var curtimes = rd.getTime();

        curtimes = curtimes + clientOffset - serverOffset;
        return new Date(curtimes);
    }
    return "";
}

function getToday() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!

    var yyyy = today.getFullYear();
    if (dd < 10) { dd = '0' + dd } if (mm < 10) { mm = '0' + mm } today = yyyy + "年" + mm + "月" + dd + "日";
    return today;
}

function check_imgfile(id) {
    //str=document.getElementById('carImg').value.toUpperCase();
    str = $("#" + id).val().toUpperCase();

    if (str == "") {
        return false;
    }

    suffix = ".JPG";
    suffix2 = ".JPEG";
    suffix3 = ".PNG";
    suffix4 = ".GIF";
    suffix5 = ".BMP";
    if (!(str.indexOf(suffix, str.length - suffix.length) !== -1 ||
            str.indexOf(suffix2, str.length - suffix2.length) !== -1 ||
            str.indexOf(suffix3, str.length - suffix3.length) !== -1 ||
            str.indexOf(suffix5, str.length - suffix5.length) !== -1 ||
            str.indexOf(suffix4, str.length - suffix4.length) !== -1)) {
        alert('文件类型不允许，\n允许文件: *.jpg，*.jpeg，*.png，*.gif，*.bmp');
        document.getElementById('id').value = '';
        return false;

    }

    return true;
}

function validateAlphaNumeric(data) {
    if (/[^a-zA-Z0-9]/.test(data)) {
        //alert('Input is not alphanumeric');
        return false;
    }
    return true;
}

function validateSpecialChar(data) {

    var iChars = "!@#$%^&*()+=-[]\\\';,./{}|\":<>?";

    for (var i = 0; i < data.length; i++) {
        if (iChars.indexOf(data.charAt(i)) != -1) {
            //alert("Your username has special characters. \nThese are not allowed.\n Please remove them and try again.");
            return false;
        }
    }

    return true;
}

function validateNumeric(data) {
    if (/[^0-9]/.test(data)) {
        //alert('Input is not alphanumeric');
        return false;
    }
    return true;
}