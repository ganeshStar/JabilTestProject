"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.CommonHandler = void 0;
var rxjs_1 = require("rxjs");
//@Injectable({ providedIn: 'root' })
var CommonHandler = /** @class */ (function () {
    function CommonHandler() {
    }
    CommonHandler.prototype.errorHandler = function (error) {
        var errorMessage = 'An error occurred while processing your request';
        if (error.error instanceof ErrorEvent) {
            // Get client-side error
            errorMessage = error.error.message;
        }
        else {
            // Get server-side error
            if (error.status !== undefined || error.message !== undefined)
                errorMessage = "Error Code: " + error.status + "\nMessage: " + error.message;
        }
        return rxjs_1.throwError(errorMessage);
    };
    return CommonHandler;
}());
exports.CommonHandler = CommonHandler;
//# sourceMappingURL=commonHandler.js.map