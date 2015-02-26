/**************** StringBuilder start  jason 2013-10-25************************/

function StringBuilder() {
    this._buffers = [];
    this._length = 0;
    this._splitChar = arguments.length > 0 ? arguments[arguments.length - 1] : '';

    if (arguments.length > 0) {
        for (var i = 0, iLen = arguments.length - 1; i < iLen; i++) {
            this.Append(arguments[i]);
        }
    }
}

StringBuilder.prototype.Append = function (str) {
    this._length += str.length;
    this._buffers[this._buffers.length] = str;
}
StringBuilder.prototype.Add = StringBuilder.prototype.append;

StringBuilder.prototype.AppendFormat = function () {
    if (arguments.length > 1) {
        var TString = arguments[0];
        if (arguments[1] instanceof Array) {
            for (var i = 0, iLen = arguments[1].length; i < iLen; i++) {
                var jIndex = i;
                var re = eval("/\\{" + jIndex + "\\}/g;");
                TString = TString.replace(re, arguments[1][i]);
            }
        }
        else {
            for (var i = 1, iLen = arguments.length; i < iLen; i++) {
                var jIndex = i - 1;
                var re = eval("/\\{" + jIndex + "\\}/g;");
                TString = TString.replace(re, arguments[i]);
            }
        }
        this.Append(TString);
    }
    else if (arguments.length == 1) {
        this.Append(arguments[0]);
    }
}

StringBuilder.prototype.Length = function () {
    if (this._splitChar.length > 0 && (!this.IsEmpty())) {
        return this._length + (this._splitChar.length * (this._buffers.length - 1));
    }
    else {
        return this._length;
    }
}

StringBuilder.prototype.IsEmpty = function () {
    return this._buffers.length <= 0;
}

StringBuilder.prototype.Clear = function () {
    this._buffers = [];
    this._length = 0;
}

StringBuilder.prototype.ToString = function () {
    if (arguments.length == 1) {
        return this._buffers.join(arguments[1]);
    }
    else {
        return this._buffers.join(this._splitChar);
    }
}


/**************** StringBuilder end jason 2013-10-25************************/


