// JavaScript by 20script.ir
// tooltip
this.tooltip = function () {
    /* CONFIG */
    xOffset = 10;
    yOffset = 20;
    /* END CONFIG */
    $(".tooltip").hover(function (e) {
        this.t = this.title;
        this.title = "";
        $("body").append("<p id='tooltip'>" + this.t + "</p>");
        $("#tooltip")
			.css("top", (e.pageY - xOffset) + "px")
			.css("left", (e.pageX + yOffset) + "px")
			.fadeIn("fast");
    },
	function () {
	    this.title = this.t;
	    $("#tooltip").remove();
	});
    $(".tooltip").mousemove(function (e) {
        $("#tooltip")
			.css("top", (e.pageY - xOffset) + "px")
			.css("left", (e.pageX + yOffset) + "px");
    });
};



// 
$(document).ready(function () {
    $("div.loading").delay(1800).fadeOut(200);
    tooltip();
});






$(document).ready(function () {

    function runIt() {
        $("div#slide1").delay(500).fadeIn(300, function () {
            $("div#slide1").delay(6000).fadeOut(300, function () {
                $("div#slide2").delay(500).fadeIn(300, function () {
                    $("div#slide2").delay(6000).fadeOut(300, function () {
                        $("div#slide3").delay(500).fadeIn(300);
                        $("div#slide3").delay(6000).fadeOut(300, function () {
                            $("div#slide4").delay(500).fadeIn(300);
                            $("div#slide4").delay(500).fadeOut(300, function () {
                                $("div#slide5").delay(500).fadeIn(300);
                                $("div#slide5").delay(500).fadeOut(300,runIt);
                            });
                        });
                    });
                });


            });
        });

    }

    runIt();


    $(window).scroll(function () {
        // 
        var h = $('#wrap').height();
        var y = $(window).scrollTop();
        if (y > (120)) {
            // ----
            $("#message").slideDown(500);
        } else {
            $('#message').slideUp(500);
        }
    });
})