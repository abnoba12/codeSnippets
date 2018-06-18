/**
 * @description Timer function that counts in seconds 
 * @param {Knockout Observerable} timerSeconds Time elapsed since start() in seconds
 * @param {Knockout Observerable} timerFormatted (Optional) Time elapsed formatted in h:mm:ss
* @param {number} countDown (Optional) Makes the timer count down from the countDown number provided
 */
function timer(timerSeconds, timerFormatted, countDown) {
    var self = this;
    var updateTimer = function () {
        var time = moment().utcOffset(0);
        time.set({ hour: 0, minute: 0, second: timerSeconds(), millisecond: 0 });

        if (time.hour() > 0 && timerSeconds() >= 0 && timerFormatted) {
            timerFormatted(time.format('h:mm:ss'));
        } else if (timerSeconds() >= 0 && timerFormatted) {
            timerFormatted(time.format('m:ss'));
        }

        if (countDown) {
            timerSeconds(timerSeconds() - 1);
        } else {
            timerSeconds(timerSeconds() + 1);
        }
    };

    var init = function () {
        self.Timer = $.timer(updateTimer, 1000, true);
    };

    self.stopAndReset = function () {
        timerSeconds(undefined);
        timerFormatted ? timerFormatted(undefined) : undefined;
        if (self.Timer) {
            self.Timer.stop().once();
        }
    };

    self.start = function () {
        timerSeconds(countDown ? countDown : 0);
        $(init);
    };
}