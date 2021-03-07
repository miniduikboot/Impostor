var SERVER_PORT = 22023;

var DEFAULT_KEYS = {
    // Version 2021.3.5s
    steam: { selected: 'NHKLLGFLCLM', servers: 'PBKMLNEHKHL' },
    // Version 2021.3.5a
    android: { selected: 'PFJBDMNLKOC', servers: 'LIDGKFEKEMH' },
    // Version 2021.3.5i
    itch: { selected: 'FMCHICBDPCE', servers: 'HACEEIOGMDC' },
    // Version 2021.3.5e
    epic: { selected: 'KCEOFOCILEP', servers: 'FHHFNNEMCJC' },
    // Version 2021.2.21m
    win10: { selected: 'BIPBMPFJBMI', servers: 'GECPHDGCFJM' },
}

$(document).ready(function() {
    fillIPAdressUsingLocationHash();

    showPlatformText();

    $("#serverFileForm").submit(function(e) {
        e.preventDefault();
        let serverIp = $("#ip").val();
        // Ports cannot be changed in the current version
        //let serverPort = $("#port").val();
        let serverFqdn = $("#fqdn").val();
        let serverName = $("#name").val();
        if (serverName == "") {
            serverName = "Impostor";
        }
        let platform = $("#keys-radio input[type='radio']:checked").attr("value")
        let keys = DEFAULT_KEYS[platform];
        let serverFileBytes = generateServerFile(serverName, serverIp, serverFqdn, keys);
        let blob = new Blob([serverFileBytes.buffer]);
        saveFile(blob, "regionInfo.json");
    });

});

function fillIPAdressUsingLocationHash() {
    let urlServerAddress = document.location.hash.substr(1).split(":");
    let serverIp = urlServerAddress[0];
    let serverPort = urlServerAddress.length > 1 ? urlServerAddress[1] : SERVER_PORT.toString();
    const ipPattern = $("#ip").attr("pattern");

    if (new RegExp(ipPattern).test(serverIp)) {
        $("#ip").val(serverIp);
    }
    if (new RegExp("^[0-9]+$", "g").test(serverPort)) {
        $("#port").val(serverPort);
    }
}

function showPlatformText() {
    if (navigator.userAgent.match(/iPhone/i) || navigator.userAgent.match(/iPod/i)) {
        $('.ios-support').show();
    } else if (navigator.userAgent.match(/android/i)) {
        $('.android-support').show();
        $("#keys-android").prop("checked", true)
    } else {
        $('.desktop-support').show();
        $("#keys-steam").prop("checked", true)
    }
}

function generateServerFile(name, ip, fqdn, keys) {
    let server = new Map();
    server['$type'] = 'DnsRegionInfo, Assembly-CSharp';
    server['Fqdn'] = fqdn;
    server['DefaultIp'] = ip;
    server['Name'] = name;
    server['TranslateName'] = 1003;

    let file = new Map();
    file[keys.selected] = 0;
    file[keys.servers] = [server];
    return new Uint8Array(stringToBytes(JSON.stringify(file)));
}

function saveFile(blob, fileName) {
    let url = URL.createObjectURL(blob);

    let a = document.createElement("a");
    document.body.appendChild(a);
    a.style = "display: none";
    a.href = url;
    a.download = fileName;
    a.click();

    URL.revokeObjectURL(url);
}

function stringToBytes(str) {
    let bytes = [];
    for (let i = 0; i < str.length; i++) {
        bytes.push(str.charCodeAt(i));
    }
    return bytes;
}
