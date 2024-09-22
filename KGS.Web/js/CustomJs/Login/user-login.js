(function ($) {
    function Index() {
        var $this = this, userLogin;
        function initializeForm() {
            userLogin = new Global.FormHelper($("#formLogin"), {
                updateTargetId: "validation-summary", validateSettings: { ignore: '' }
            }, function onSucccess(result) {
                
                window.location.href = result.redirectUrl;
            });
        }

        $this.init = function () {
            initializeForm();

        };
    }

    $(function () {
        var self = new Index();
        self.init();
    });
}(jQuery));