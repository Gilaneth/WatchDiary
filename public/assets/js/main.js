/*
	ZeroFour by HTML5 UP
	html5up.net | @ajlkn
	Free for personal and commercial use under the CCA 3.0 license (html5up.net/license)
*/

(function($) {

	var	$window = $(window),
		$body = $('body');

	// Breakpoints.
		breakpoints({
			xlarge:  [ '1281px',  '1680px' ],
			large:   [ '981px',   '1280px' ],
			medium:  [ '737px',   '980px'  ],
			small:   [ null,      '736px'  ]
		});

	// Play initial animations on page load.
		$window.on('load', function() {
			window.setTimeout(function() {
				$body.removeClass('is-preload');
			}, 100);
		});

	// Dropdowns.
		$('#nav > ul').dropotron({
			offsetY: -22,
			mode: 'fade',
			noOpenerFade: true,
			speed: 300,
			detach: false
		});

	// Nav.

		// Title Bar.
			$(
				'<div id="titleBar">' +
					'<a href="#navPanel" class="toggle"></a>' +
					'<span class="title">' + $('#logo').html() + '</span>' +
				'</div>'
			)
				.appendTo($body);

		// Panel.
			$(
				'<div id="navPanel">' +
					'<nav>' +
						$('#nav').navList() +
					'</nav>' +
				'</div>'
			)
				.appendTo($body)
				.panel({
					delay: 500,
					hideOnClick: true,
					hideOnSwipe: true,
					resetScroll: true,
					resetForms: true,
					side: 'left',
					target: $body,
					visibleClass: 'navPanel-visible'
				});

    function decodeJwtPayload(token) {
        var base64Url = token.split('.')[1];
        var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        var padded = base64 + '='.repeat((4 - (base64.length % 4)) % 4);
        return JSON.parse(atob(padded));
    }

    function updateAuthSlot() {
        var slot = document.getElementById('auth-slot');
        if (!slot) return;
        var token = localStorage.getItem('watchdiary_token');
        if (!token) {
            slot.innerHTML = '<a href="/pages/login.html">УВІЙТИ</a>';
            return;
        }
        try {
            var payload = decodeJwtPayload(token);
            var username = payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'];
            slot.innerHTML =
                '<a href="/pages/profile.html">' + username + '</a> &nbsp;' +
                    '<a href="#" onclick="logout(); return false;" style="opacity:0.7;">ВИЙТИ</a>';
        } catch (e) {
            localStorage.removeItem('watchdiary_token');
            slot.innerHTML = '<a href="/pages/login.html">УВІЙТИ</a>';
        }
    }

    function logout() {
        localStorage.removeItem('watchdiary_token');
        window.location.href = '/index.html';
    }

    window.updateAuthSlot = updateAuthSlot;
    window.logout = logout;

})(jQuery);
