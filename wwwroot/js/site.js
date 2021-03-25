// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
/**
 * 
 * The javascript / jquery code here is mostly for handling the create and edit recipe form
 * We need to allow the user to add as many ingredients, instructions, and equipment as needed.
 * 
 * I accomplished this by assigning a div with the input elements to a generic "variable-input-group" class
 * 
 */
$(function () {
    // updates the Instruction Step read only inputs after each change
    function SetInstructionStepValues() {
        var step = 1;
        $('input[name="InstructionStep"]').each(function () {
            $(this).val(step);
            ++step;
        })
    }

    /**
     * Add on Click handlers to the remove button and add buttons
     * 
     * These functions are catch all for adding equipment, ingredients, or instructions
     * 
     * the class names are very important for these to work properly
     */
    $('.remove-btn').on('click', function (e) {
        e.preventDefault();
        // the button's parent's parent should be the div with our .variable-input-group class
        $(e.currentTarget).parent().parent().remove();
        // update the instruction steps if needed
        SetInstructionStepValues();
    });

    $('.add-btn').on('click', function (e) {
        e.preventDefault();
        // the button's parent's parent should be the div with our .variable-input-group class
        // create a clone of the last group
        var $lastInputGroup = $(e.currentTarget).parent().parent().siblings('.variable-input-group').last();
        // do a deep clone with data and events so that the new remove buttons also get the proper callbacks
        var $newInputGroup = $lastInputGroup.clone(true, true);

        // set remaining input values to empty
        $newInputGroup.find('input').val("");
        $newInputGroup.find('textarea').val("");

        // need to call this to actually show the new input group
        $newInputGroup.insertAfter($lastInputGroup);

        // update the instruction step values
        SetInstructionStepValues();
    });
})