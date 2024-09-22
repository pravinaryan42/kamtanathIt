/**
 * This plug-in removes the default behaviour of DataTables to filter on each
 * keypress, and replaces with it the requirement to press the enter key to
 * perform the filter. 06-09-2017
 */


$.fn.dataTableExt.oApi.fnFilterOnReturn = function (oSettings) {
    var _that = this;
    this.each(function (i) {
        $.fn.dataTableExt.iApiIndex = i;
        var $this = this;
        var anControl = $('input', _that.fnSettings().aanFeatures.f);
        anControl
			.unbind('keyup search input')
			.bind('keypress', function (e) {
			    if (e.which == 13) {
			        $.fn.dataTableExt.iApiIndex = i;
			        _that.fnFilter(anControl.val());
			    }
			});
        return this;
    });
    return this;
};

var couter = 0;
function bindEnterEventInDataTableJSGrid(isManageInterval) {
    isManageInterval = isManageInterval != null && isManageInterval != undefined && isManageInterval;
    if ($.fn.DataTable.fnTables() && $.fn.DataTable.fnTables().length > 0) {
        if (isManageInterval) {
            clearInterval(_intervalDataTable);
        }
        $.each($.fn.DataTable.fnTables(), function (i, v) {
            var _id = $(v).attr('id');
            $('#' + _id).dataTable().fnFilterOnReturn();
        });
    }
}
var _intervalDataTable = setInterval(function () {
    bindEnterEventInDataTableJSGrid(true);
    if (couter > 100) {
        clearInterval(_intervalDataTable);
    }
    couter++;
}, 100);
$(document).on("change", "select.dataTableSearchByTeam", function () {
    bindEnterEventInDataTableJSGrid(false);
});


