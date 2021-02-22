// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    // updates the Instruction Step read only inputs after each change
    function SetInstructionStepValues() {
        var step = 1;
        $('input[name="InstructionStep"]').each(function () {
            $(this).val(step);
            ++step;
        })
    }

    $('.remove-btn').on('click', function (e) {
        e.preventDefault();
        $(e.currentTarget).parent().parent().remove();

        SetInstructionStepValues();
    });

    $('.add-btn').on('click', function (e) {
        e.preventDefault();
        // create a clone of the last group
        var $lastInputGroup = $(e.currentTarget).parent().parent().siblings('.variable-input-group').last();
        // do a deep clone with data and events so that the new remove buttons also get the proper callbacks
        var $newInputGroup = $lastInputGroup.clone(true, true);


        // set remaining input values to empty
        $newInputGroup.find('input').val("");
        $newInputGroup.find('textarea').val("");
        $newInputGroup.insertAfter($lastInputGroup);
        SetInstructionStepValues();
    });
})