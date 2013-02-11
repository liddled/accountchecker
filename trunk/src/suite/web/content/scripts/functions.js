$(function(){new dl.accountchecker.DatePicker();});

var dl = dl || {};
dl.accountchecker = dl.accountchecker || {};

dl.accountchecker.urls = dl.accountchecker.urls || {}
dl.accountchecker.urls.category_lookup = 'categories/lookup/';

dl.accountchecker.options = dl.accountchecker.options || {}
dl.accountchecker.options.datePickerSelector = '.dl-ui-date';
dl.accountchecker.options.dateFormat = 'dd MM yy';

dl.accountchecker.DatePicker = function (dateFormat)
{
    this.elements = dl.accountchecker.options.datePickerSelector;
    this.dateFormat = dateFormat || dl.accountchecker.options.dateFormat;
    this.init_();
};

dl.accountchecker.DatePicker.prototype.init_ = function ()
{
    $(this.elements).removeClass('hasDatepicker').datepicker(
    {
        changeYear: true,
        changeMonth: true,
        dateFormat: this.dateFormat
    });
};

dl.accountchecker.DatePeriodPicker = function (dateFormat)
{
    this.textboxFrom = $('input[id="tb-date-from"]');
    this.textboxTo = $('input[id="tb-date-to"]');
    this.dateFormat = dateFormat || dl.accountchecker.options.dateFormat;
    this.init_();
};

dl.accountchecker.DatePeriodPicker.prototype.init_ = function ()
{
    var _this = this;

    $(_this.textboxFrom).removeClass('hasDatepicker').datepicker(
    {
        changeYear: true,
        changeMonth: true,
        dateFormat: this.dateFormat
    }).change(function ()
    {
        var date = _this.textboxFrom.datepicker('getDate');
        var sort8Date = _this.getSort8Date(date);
        $('#from').val(sort8Date);
    });

    $(_this.textboxTo).removeClass('hasDatepicker').datepicker(
    {
        changeYear: true,
        changeMonth: true,
        dateFormat: this.dateFormat
    }).change(function ()
    {
        var date = _this.textboxTo.datepicker('getDate');
        var sort8Date = _this.getSort8Date(date);
        $('#to').val(sort8Date);
    });
};

dl.accountchecker.DatePeriodPicker.prototype.getSort8Date = function (date)
{
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var date = date.getDate();

    if (month < 10) month = "0" + month;
    if (date < 10) date = "0" + date;

    return year.toString() + month.toString() + date.toString();
};

dl.accountchecker.CategorySelector = function (textbox)
{
    this.textbox = textbox || ".category-selector";
    this.init_();
};

dl.accountchecker.CategorySelector.prototype.init_ = function ()
{
    var obj = this;
    $(this.textbox).bind("keydown", function (event)
    {
        if (event.keyCode === $.ui.keyCode.TAB && $(this).data("autocomplete").menu.active)
        {
            event.preventDefault();
        }
    })
    .autocomplete(
    {
        source: function (request, response)
        {
            $.getJSON(dl.accountchecker.urls.category_lookup, { term: obj.extractLast(obj, request.term) }, response);
        },
        focus: function ()
        {
            return false;
        },
        search: function ()
        {
            var term = obj.extractLast(obj, this.value);
            if (term.length < 1)
                return false;
        },
        select: function (event, ui)
        {
            var terms = obj.split(this.value);
            terms.pop();
            terms.push(ui.item.value);
            terms.push("");
            this.value = terms.join(" ");
            return false;
        }
    });
};

dl.accountchecker.CategorySelector.prototype.split = function (val)
{
    return val.split(" ");
};

dl.accountchecker.CategorySelector.prototype.extractLast = function (obj, term)
{
    return obj.split(term).pop();
};
