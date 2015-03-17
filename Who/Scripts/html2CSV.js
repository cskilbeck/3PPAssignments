jQuery.fn.table2CSV = function(options) {
    var options = jQuery.extend({
        separator: ',',
        header: []
    },
    options);

    var csvData = [];

    var headerArr = [];
    var el = this;

    //header
    var numCols = options.header.length;
    var tmpRow = []; // construct header available array

    if (numCols > 0) {
        for (var i = 0; i < numCols; i++) {
            tmpRow[tmpRow.length] = formatData(options.header[i]);
        }
    } else {
        $(el).filter(':visible').find('th').each(function() {
            if ($(this).css('display') != 'none') tmpRow[tmpRow.length] = formatData($(this).html());
        });
    }

    row2CSV(tmpRow);

    // actual data
    $(el).find('tr').each(function() {
        var tmpRow = [];
        $(this).filter(':visible').find('td').each(function() {
            if ($(this).css('display') != 'none') tmpRow[tmpRow.length] = formatData($(this).html());
        });
        row2CSV(tmpRow);
    });

    var bom = "\uFEFF";
    var s = csvData.join('\n')
    return bom.concat(s);

    function row2CSV(tmpRow) {
        var tmp = tmpRow.join('') // to remove any blank rows
        // alert(tmp);
        if (tmpRow.length > 0 && tmp != '') {
            var mystr = tmpRow.join(options.separator);
            csvData[csvData.length] = mystr;
        }
    }
    function formatData(input) {
        // replace " with “
        input = input.trim();
        var regexp = new RegExp(/["]/g);
        var output = input.replace(regexp, "“");
        //HTML
        var regexp = new RegExp(/\<[^\<]+\>/g);
        var output = output.replace(regexp, "");
        if (output == "") return '';
        return '"' + output + '"';
    }
};

window.saveAs || (window.saveAs = (window.navigator.msSaveBlob ? function (b, n) { return window.navigator.msSaveBlob(b, n); } : false) || window.webkitSaveAs || window.mozSaveAs || window.msSaveAs || (function () {

    // URL's
    window.URL || (window.URL = window.webkitURL);

    if (!window.URL) {
        return false;
    }

    return function (blob, name) {
        var url = URL.createObjectURL(blob);

        // Test for download link support
        if ("download" in document.createElement('a')) {

            var a = document.createElement('a');
            a.setAttribute('href', url);
            a.setAttribute('download', name);

            // Create Click event
            var clickEvent = document.createEvent("MouseEvent");
            clickEvent.initMouseEvent("click", true, true, window, 0,
				event.screenX, event.screenY, event.clientX, event.clientY,
				event.ctrlKey, event.altKey, event.shiftKey, event.metaKey,
				0, null);

            // dispatch click event to simulate download
            a.dispatchEvent(clickEvent);

        }
        else {
            // fallover, open resource in new tab.
            window.open(url, '_blank', '');
        }
    };

})());