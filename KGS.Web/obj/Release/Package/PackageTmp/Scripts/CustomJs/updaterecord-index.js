(function ($) {

    function UpdateRecordIndex() {
        var $this = this, grid, formAddEdit, formAddEditAllergy;


        $this.init = function () {
            formAddEditAllergy = new Global.FormHelperWithFiles($("#formAddEditAllergy"),
                {
                    updateTargetId: "validation-summary"
                }, function onSuccess(result) {
                    window.location.reload();
                });

        };
    }
    $(function () {
        var self = new UpdateRecordIndex();
        self.init();
    });

}(jQuery))

