exports.config = {
    framework: 'jasmine',
    seleniumAddress: 'http://localhost:4444/wd/hub',
    specs: ['upper-nav.js'],
    multiCapabilities: [{
        browserName: 'chrome',
        chromeOptions: {
            args: ['--window-size=1280,1000']
        }
    }],
    jasmineNodeOpts: {
        showColors: true,
        defaultTimeoutInterval: 30000
    }
}