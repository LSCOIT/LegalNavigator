/*
 * For more info on creating custom param type, check https://docs.cucumber.io/cucumber/cucumber-expressions/#custom-parameter-types
 * And existing issue, https://github.com/cucumber/cucumber-js/issues/927
 */
let { defineParameterType } = require('cucumber');
defineParameterType({
    regexp: /"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)"/,
    name: 'url',
    transformer: function (s) {
        return s; // transformer is irrelevant  
    }
});
