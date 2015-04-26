//This controls the percentage of how many visitors will have the popout displayed to them by default. 
//If you want it closed for everyone by default set this to 0 or displayed for everyone 100
var popoutPromoPercentageDisplayed = 75;

//Set the "don't show" flag based on the show percentage
var number = 1 + Math.floor(Math.random() * 100);
if(number > popoutPromoPercentageDisplayed){
	//Don't show the popout
}	