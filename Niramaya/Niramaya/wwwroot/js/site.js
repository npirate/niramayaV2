// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const $dropdown = $(".dropdown");
const $dropdownToggle = $(".dropdown-toggle");
const $dropdownMenu = $(".dropdown-menu");
const showClass = "show";

$(window).on("load resize", function () {
    if (true) {
        $dropdown.hover(
            function () {
                const $this = $(this);
                $this.addClass(showClass);
                $this.find($dropdownToggle).attr("aria-expanded", "true");
                $this.find($dropdownMenu).addClass(showClass);
            },
            function () {
                const $this = $(this);
                $this.removeClass(showClass);
                $this.find($dropdownToggle).attr("aria-expanded", "false");
                $this.find($dropdownMenu).removeClass(showClass);
            }
        );
    } else {
        $dropdown.off("mouseenter mouseleave");
    }
});
//Now starts datepicker and time scripts for profile page

$('input[name ^="from"]').timepicker({ 'timeFormat': 'H:i:s', disableTextInput: true });
$('input[name ^="to"]').timepicker({ 'timeFormat': 'H:i:s', disableTextInput: true });

$('i[name="add"]').click(function () {
    $(this).parent().next().removeClass('d-none');
});

$('i[name="minus"]').click(function () {
    $(this).siblings('input').val('');
    $(this).parent().addClass('d-none');
});

$('#profileform').submit(function () {
    var isValid = $('input[name ="DayChecked"]:checked').length == 0;
    if (isValid) {
        $("#dayerrorcheck").show(); // display error message
        return false; // prevent submit
    } else {
        $("#dayerrorcheck").hide();
        if (timeErrorcheck() === 1) {
            $("#timeerrorcheck").show();
            return false;
        }
        else
            return true;
    }
});

function GetHours(d) {
    var h = parseInt(d.split(':')[0]);
    return h;
}
function GetMinutes(d) {
    return parseInt(d.split(':')[1]);
}

function timeErrorcheck() {//return 1 if error , 0 if success
    var fromTimes = $('input[name ="from1"]').map(function () {
        return this.value; // simple but you could also use $(this).val();
    }).get();

    var toTimes = $('input[name ="to1"]').map(function () {
        return this.value; // simple but you could also use $(this).val();
    }).get();

    if ((fromTimes[0] === "" && toTimes[0] === "") || (fromTimes[0] === "" && toTimes[0] != "") || (fromTimes[0] != "" && toTimes[0] === "")) {
        return 1;
    } else if ((fromTimes[1] === "" && toTimes[1] != "") || (fromTimes[1] != "" && toTimes[1] === "")) {
        return 1;
    } else if ((fromTimes[2] === "" && toTimes[2] != "") || (fromTimes[2] != "" && toTimes[2] === "")) {
        return 1;
    } else if (fromTimes[0] != "" && toTimes[0] != "") {
        var f0 = new Date().setHours(GetHours(fromTimes[0]), GetMinutes(fromTimes[0]), 0);
        var t0 = new Date().setHours(GetHours(toTimes[0]), GetMinutes(toTimes[0]), 0);
        if (f0 > t0)
            return 1
    }
    else if (fromTimes[1] != "" && toTimes[1] != "") {
        var f1 = new Date().setHours(GetHours(fromTimes[1]), GetMinutes(fromTimes[1]), 0);
        var t1 = new Date().setHours(GetHours(toTimes[1]), GetMinutes(toTimes[1]), 0);
        if (f1 > t1)
            return 1
    } else if (fromTimes[2] != "" && toTimes[2] != "") {
        var f2 = new Date().setHours(GetHours(fromTimes[2]), GetMinutes(fromTimes[2]), 0);
        var t2 = new Date().setHours(GetHours(toTimes[2]), GetMinutes(toTimes[2]), 0);
        if (f2 > t2)
            return 1
    } else if (fromTimes[1] === "" && fromTimes[2] != "") {
        return 1
    }
    else if (fromTimes[1] != "" && toTimes[1] != "") {

        var t0 = new Date().setHours(GetHours(toTimes[0]), GetMinutes(toTimes[0]), 0);
        var f1 = new Date().setHours(GetHours(fromTimes[1]), GetMinutes(fromTimes[1]), 0);
        if ((f1 < t0))
            return 1
    } else if (fromTimes[2] != "" && toTimes[2] != "") {

        var t1 = new Date().setHours(GetHours(toTimes[1]), GetMinutes(toTimes[1]), 0);
        var f2 = new Date().setHours(GetHours(fromTimes[2]), GetMinutes(fromTimes[2]), 0);
        if ((f2 < t1))
            return 1
    }
    else {
        return 0;
    }
}