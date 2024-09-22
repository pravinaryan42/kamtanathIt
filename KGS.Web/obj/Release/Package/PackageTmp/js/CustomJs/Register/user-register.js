(function ($) {
    function Index() {
        var $this = this, userLogin;
        function initializeForm() {
            userLogin = new Global.FormHelper($("#formSignup"), {
                updateTargetId: "validation-summary", validateSettings: { ignore: '' }
            }, function onSucccess(result) {
                
                window.location.href = result.redirectUrl;
            });


            $("#PhoneNumber").on("keyup", function () {
                var valid = /^\d{0,10}(\.\d{0,2})?$/.test(this.value),
                    val = this.value;
                if (!valid) {
                    this.value = val.substring(0, val.length - 1);
                }
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