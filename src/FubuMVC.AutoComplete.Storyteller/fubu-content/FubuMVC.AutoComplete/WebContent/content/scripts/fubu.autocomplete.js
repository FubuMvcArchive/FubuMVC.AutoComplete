(function ($) {
    $.fn.lookupValues = function () {
        return this.each(function () {
            var input = $(this);
            var url = $(input).data('lookup-url');
            var backingField = $(input).data('value-for');
            $(input).attr('title', $('#' + backingField).attr('title'));

            var update = function (event, ui) {
                var guidForAutocomplete = ui.item ? ui.item.value : '';
                if (ui.item == null) {
                    guidForAutocomplete = '';
                }
                else {
                    $(this).val(ui.item.label);
                }

                $(this).prev().val(guidForAutocomplete);
                $(this).trigger('autocompleteChanged', guidForAutocomplete);
                return false;
            };

            var autocomplete = $(input).autocomplete({
                focus: function (event, ui) {
                    if (ui.item) {
                        $(this).val(ui.item.label);
                    }
                    return false;
                },
                select: update,
                change: update,
                // TODO -- get fancier here.  Use a real function to memoize
                source: function (request, response) {
                    $.ajax({
                        url: url,
                        type: 'POST',
                        data: {
                            term: request.term
                        },
                        success: function (values) {
                            response(values);
                        }
                        // we get the error reporting for free here
                    });
                }
            });

            // You've gotta be kidding. This is the workaround for fullscreen iOS since they require an href='#'
            var render = autocomplete.data("autocomplete")._renderItem;
            autocomplete.data("autocomplete")._renderItem = function (ul, item) {
                var result = render(ul, item);
                result.find('a').attr('href', '#');
                return result;
            };
        });
    };
} (jQuery));

var setupAutocompletes = function () {
    $('.autocomplete').lookupValues();
};


$(document).ready(function () {
    $(document).bind('domChanged', function () {
        setupAutocompletes();
    });
    setupAutocompletes();

    $(document).ready(function () {
        $("input:text.autocomplete").focus(function () { $(this).select(); });
    });
});