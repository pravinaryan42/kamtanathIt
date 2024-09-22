
/*global window, $*/
$.validator.addMethod(
    "regex",
    function (value, element, param) {
        var re = new RegExp(param.pattern);
        return this.optional(element) || re.test(value);
    },
    "Invalid format."
);

var Global = {
    MessageType: {
        Success: 0,
        Error: 1,
        Warning: 2,
        Info: 3
    }
};



Global.FormHelper = function (formElement, options, onSucccess, onError, onValidate) {
    "use strict";
    var settings = {};
    settings = $.extend({}, settings, options);
    formElement.validate(settings.validateSettings);


    formElement.submit(function (e) {
        e.stopPropagation();
        e.preventDefault();
        e.stopImmediatePropagation();
        if (options && options.beforeSubmit) {
            if (!options.beforeSubmit()) {
                return false;
            }
        }
        var submitBtn = formElement.find(':submit');
        if (formElement.validate().valid()) {
            var returnOnValidate = true;
            if (onValidate !== null && onValidate !== undefined && typeof (onValidate) === "function") {
                returnOnValidate = onValidate(submitBtn);
            }
            if (returnOnValidate) {
                submitBtn.find('i').removeClass("fa fa-arrow-circle-right");
                submitBtn.find('i').addClass("fa fa-refresh");
                submitBtn.prop('disabled', true);
                submitBtn.find('span').html('Submiting..');
                $.ajax(formElement.attr("action"), {
                    type: "POST",
                    data: formElement.serializeArray(),
                    success: function (result) {
                        if (onSucccess === null || onSucccess === undefined) {
                            if (result.isSuccess) {
                                window.location.href = result.redirectUrl;
                            } else {
                                if (settings.updateTargetId) {
                                    $("#" + settings.updateTargetId).html(result);
                                }
                            }
                        } else {
                            onSucccess(result);
                        }
                    },
                    error: function (jqXHR, status, error) {
                        if (onError !== null && onError !== undefined) {
                            onError(jqXHR, status, error);
                        }
                    },
                    complete: function () {
                        submitBtn.find('i').removeClass("fa fa-refresh");
                        submitBtn.find('i').addClass("fa fa-arrow-circle-right");
                        submitBtn.find('span').html('Submit');
                        submitBtn.prop('disabled', false);
                    }
                });
            }
        }

        e.preventDefault();
    });

    return formElement;
};


Global.FormHelperWithFiles = function (formElement, options, onSucccess, onError, loadingElementId, onComplete) {
    "use strict";
    var settings = {};
   
    settings = $.extend({}, settings, options);
    formElement.validate(settings.validateSettings);
    formElement.submit(function (e) {

        if (options && options.beforeSubmit) {
            if (!options.beforeSubmit()) {
                return false;
            }
        }

        var formdata = new FormData();
        formElement.find('input[type="file"]:not(:disabled)').each(function (i, elem) {
            if (elem.files && elem.files.length) {
                for (var i = 0; i < elem.files.length; i++) {
                    var file = elem.files[i];
                    formdata.append(elem.getAttribute('name'), file);
                }
            }
        });

        $.each(formElement.serializeArray(), function (i, item) {
            formdata.append(item.name, item.value);
        });


        var submitBtn = formElement.find(':submit');
        if (formElement.validate().valid()) {
            submitBtn.find('i').removeClass("fa fa-arrow-circle-right");
            submitBtn.find('i').addClass("fa fa-refresh");
            submitBtn.prop('disabled', true);
            submitBtn.find('span').html('Submiting..');
            $.ajax(formElement.attr("action"), {
                type: "POST",
                data: formdata,
                contentType: false,
                processData: false,
                beforeSend: function () {
                    if (settings.loadingElementId != null || settings.loadingElementId != undefined) {
                        $("#" + settings.loadingElementId).show();
                        submitBtn.hide();
                    }
                },
                success: function (result) {
                    if (onSucccess === null || onSucccess === undefined) {
                        if (result.isSuccess) {
                            window.location.href = result.redirectUrl;
                        } else {
                            if (settings.updateTargetId) {
                                var datatresult = (result.message == null || result.message == undefined) ? ((result.data == null || result.data == undefined) ? result : result.data) : result.message;
                                $("#" + settings.updateTargetId).html(datatresult);
                            }
                        }
                    } else {
                        onSucccess(result);
                    }
                },
                error: function (jqXHR, status, error) {
                    if (onError !== null && onError !== undefined) {
                        onError(jqXHR, status, error);
                        $("#loadingElement").hide();
                    }
                },
                complete: function (result) {
                    if (onComplete === null || onComplete === undefined) {
                        if (settings.loadingElementId != null || settings.loadingElementId != undefined) {
                            $("#" + settings.loadingElementId).hide();
                        }
                        submitBtn.find('i').removeClass("fa fa-refresh");
                        submitBtn.find('i').addClass("fa fa-arrow-circle-right");
                        submitBtn.find('span').html('Submit');
                        submitBtn.prop('disabled', false);
                    } else {
                        onComplete(result);
                    }
                }
            });
        }

        e.preventDefault();
    });

    return formElement;
};



Global.GridHelper = function (gridElement, options) {
    if ($(gridElement).find("thead tr th").length > 1) {
        var settings = {};
        settings = $.extend({}, settings, options);
        $(gridElement).dataTable(settings);
        return $(gridElement);
    }
};

Global.FormValidationReset = function (formElement, validateOption) {
    if ($(formElement).data('validator')) {
        $(formElement).data('validator', null);
    }

    $(formElement).validate(validateOption);

    return $(formElement);
};

Global.IsNull = function (o) { return typeof o === "undefined" || typeof o === "unknown" || o == null };
Global.IsNotNull = function (o) { return !Global.IsNull(o); };
Global.IsNullOrEmptyString = function (str) {
    return Global.IsNull(str) || typeof str === "string" && $.trim(str).length == 0
};
Global.IsNotNullOrEmptyString = function (str) { return !Global.IsNullOrEmptyString(str); };
Global.IsNotNullResult = function (results) { return Global.IsNotNull(results) && results.length > 0; };

Global.showsPartial = function ($url, $divId, fnCallBack) {
    $.ajax({
        url: $url,
        type: 'GET',
        async: false,
        crossDomain: true,
        cache: false,
        success: function (htmlElement) {
            $('#' + $divId).empty().html(htmlElement).promise().done(function () {
                if (fnCallBack && typeof (fnCallBack) === "function") {
                    fnCallBack(htmlElement);
                }
            });
        }
    });
}
/******************************************************************************************************************************/
Global.Alert = function (title, message, callback) {
    alertify.alert(title, message, function () {
        if (callback)
            callback();
    }).set({ transition: 'fade' });
};



Global.Confirm = function (title, message, okCallback, cancelCallback) {
    return alertify.confirm(title, message, function () {
        if (okCallback)
            okCallback();
    }, function () {
        if (cancelCallback)
            cancelCallback();
    }).set({ transition: 'fade', 'closable': false });
};

Global.ShowMessage = function (message, type) {
    if (type == Global.MessageType.Success) {
        alertify.success(message);
    }
    else if (type == Global.MessageType.Error) {
        alertify.error(message);
    }
    else if (type == Global.MessageType.Warning) {
        alertify.warning(message);
    }
    else if (type == Global.MessageType.Info) {
        alertify.message(message);
    }
}

Global.SetDatePicker = function (selector, endDate = null) {

    var todayDate = new Date();
    $(selector).each(function () {
        try {
            if (endDate == null) {
                $(selector).datetimepicker({
                    defaultDate: new Date(),
                    format: "MM/DD/YYYY"
                }).on('dp.change', function (e) {
                    $(selector).removeClass('error').next('label').remove();
                    $('#PersonalInfoTabContent .dobError').html('');
                });
            }
            else {
                $(selector).datetimepicker({
                    defaultDate: new Date(),
                    format: "MM/DD/YYYY",
                    maxDate: endDate
                }).on('dp.change', function (e) {
                    $(selector).removeClass('error').next('label').remove();
                    $('#PersonalInfoTabContent .dobError').html('');
                });
            }
        } catch (e) {
            console.log(e);
        }         
    })

}
Global.manageDependentCtrlVisibility = function ($item) {
    var dependentCtrl = $item.data("dependent-ctrl");
    var $collapseIcon = $item.parents("div:first").find("button[data-widget='collapse'] i");
    if ($item.is(":checked")) {
        //$('#' + dependentCtrl).removeClass("hidden");
        $('#' + dependentCtrl).slideDown("fast", function () {
            $collapseIcon.removeClass("fa-minus").addClass("fa-minus");
        });
    }
    else {
        //$('#' + dependentCtrl).addClass("hidden");        
        $('#' + dependentCtrl).slideUp("fast", function () {
            $("#" + dependentCtrl).find("input[type=text]").each(function () {
                $(this).val("");
            })
            $collapseIcon.removeClass("fa-minus").addClass("fa-plus");
        });
    }
};

/**
* Get the value of a querystring
* @param  {String} field The field to get the value of
* @param  {String} url   The URL to get the value from (optional)
* @return {String}       The field value
*/
var getQueryString = function (field, url) {
    var href = url ? url : window.location.href;
    var reg = new RegExp('[?&]' + field + '=([^&#]*)', 'i');
    var string = reg.exec(href);
    return string ? string[1] : null;
};
/******************************************************************************************************************************/

function manageDependent() {
    $("input[type=checkbox][data-dependent-ctrl!='']").each(function () {
        Global.manageDependentCtrlVisibility($(this));
    });
}
$(document).ready(function () {
    manageDependent();
    $(document).on("change", "input[type=checkbox][data-dependent-ctrl!='']", function () {
        Global.manageDependentCtrlVisibility($(this));
    });
});
//var diagnosisItemIndex = [];
var randeredItems = [];

Global.Select2Tab = function (controlId, mainCollectionId, otherCollectionId, placeholderText, multiSelectData, onChangeCallback) {
    if (controlId != null) {
        function format(state, element) {
            var isExistsInmultiSelectData = function (textToSearch) {
                var result = multiSelectData.filter(function (e, i) {
                    return e.text.trim().replace(/ /g, '').toLowerCase() == textToSearch.trim().replace(/ /g, '').toLowerCase();
                });
                return result.length >= 1;
            }
            
            if (!state.id) return state.text; // optgroup
            var isOtherItem = state.id.indexOf("O_") >= 0;
            var isMainItem = !isOtherItem && state.id.indexOf("M_") >= 0 && isExistsInmultiSelectData(state.text);
            var isNewItem = !isMainItem && !isOtherItem;
            var itemId = !isNewItem ? state.id.substring(2, state.id.length) : state.id;
            var itemControlId = isNewItem ? otherCollectionId + "_New" : isOtherItem ? otherCollectionId : mainCollectionId;

            if (!isMainItem) {
                $(element).addClass("select2-other-item");
            }

            var newIndex = 0;
            var newItemToBeRandered = $('<input id="' + itemControlId + '_' + newIndex + '__Value" name="' + itemControlId + '[' + newIndex + '].Value" data-type-value="' + state.id + '" type="hidden" class="' + itemControlId + '" value="' + itemId + '">'
                + '&nbsp; &nbsp; &nbsp;<label> ' + state.text + ' </label>');

            return newItemToBeRandered;
        }
        var resetRanderedItemIndexing = function () {
            randeredItems = [];
            $("input[type=hidden]." + mainCollectionId).each(function (i, e) {
                $(this).attr("id", mainCollectionId + "_" + i + "__Value").attr("name", mainCollectionId + "[" + i + "].Value");
            });
            $("input[type=hidden]." + otherCollectionId).each(function (i, e) {
                $(this).attr("id", otherCollectionId + "_" + i + "__Value").attr("name", otherCollectionId + "[" + i + "].Value");
            });
            $("input[type=hidden]." + otherCollectionId + "_New").each(function (i, e) {
                var newOtherControlId = otherCollectionId + "_New";
                $(this).attr("id", newOtherControlId + "_" + i + "__Value").attr("name", newOtherControlId + "[" + i + "].Value");
            });
        };

        if ($("#" + controlId).hasClass("select2-hidden-accessible")) {
            $("#" + controlId).select2('destroy');
        }

        $("#" + controlId).select2({
            placeholder: placeholderText,
            tags: true,
            templateSelection: format,
            createSearchChoice: function (term, data) { if ($(data).filter(function () { return this.text.localeCompare(term) === 0; }).length === 0) { return { id: term, text: term }; } },
            multiple: true,
            width: "100%",
            closeOnSelect: true,
            //maximumSelectionLength: 10,
            //allowClear: true,
            //minimumInputLength: 1,
            data: multiSelectData
        }).on("change", function (e) {
            resetRanderedItemIndexing();
            if (typeof (onChangeCallback) === "function") {
                setTimeout(function () {
                    onChangeCallback(e);
                }, 100);
            }
        });
        setTimeout(function () {
            resetRanderedItemIndexing();
        }, 500);

    }
};

//$.validator.addMethod(
//    "regex",
//    function (value, element, param) {
//        var re = new RegExp(param.pattern);
//        return this.optional(element) || re.test(value);
//    },
//    "Invalid format."
//);
