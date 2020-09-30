#Numpad0::
	ETH("0x28c90CaEfF6064E1E90DD44E315DefB8bf67FbD3") ;celsius
	Sleep 250
	ETC("0x6a2a64467Fc04332781Cd7483642203C8a7BD6A3") ;celsius
	Sleep 250
	XRP("rDgDuTLEtNXKhGHPZPwYN9LjZcJXCbyCEz-1769542726") ;celsius
	Sleep 250
	LTC("ltc1q3rxefn5keqtdvn9sp52f4rnt8tqewjtjlmdk3z") ;celsius
	Sleep 250
	XMR("88vNRXQw8sfVpBVK4XPMbUbFT56cceqtGHPrgaLmS6ey37eFFQzn82NfKN9rtg1zuiHJr6RuGHPeWLC63KDMPxEgCqwhsN9") ;nicehash
	Sleep 250
	ZEC("t1W44yMmRF8Wtqu6k8P18JfFDzsAwi1r338") ;celsius
	Sleep 250
return

#Numpad1::
	;ETH("0x6Ef4Ee3f12DF29352E94a38AE3E6f602B2b6570d") ;coinbase
	;Sleep 250
	;ETH("0x03d7a2a1d95dc274334c92d78e3c05fc013daf85") ;nicehash
	;Sleep 250	
	;ETH("0xeFA45fEBE30DAEd7162054D0766172f6EA876c65") ;nexo
	;Sleep 250
	ETH("0x28c90CaEfF6064E1E90DD44E315DefB8bf67FbD3") ;celsius
	Sleep 250
	DASH("Xs3SkCVQviQ3cS5EvsB7u7fHSTHmGnSPWX") ;celsius
	Sleep 250
return

#Numpad2::		
	;ETC("0x2Cb9a624E9FBA74694c50791eC388d90BAb9AAFD") ;coinbase
	;Sleep 250	
	ETC("0x6a2a64467Fc04332781Cd7483642203C8a7BD6A3") ;celsius
	Sleep 250
return

#Numpad3::
	;XRP("rw2ciyaNshpHe7bCHo4bRWq6pqqynnWKQg-2133520263") ;coinbase
	;Sleep 250
	;XRP("rGZV96HKpRbPCHbeYdtqE8Z5F74ULZ8o7E-6825") ;nicehash
	;Sleep 250
	;XRP("rnuPTVikw8HKK4hBGCtnq2J2433VYaZPZQ-1033847") ;nexo
	;Sleep 250
	XRP("rDgDuTLEtNXKhGHPZPwYN9LjZcJXCbyCEz-1769542726") ;celsius
	Sleep 250
return

#Numpad4::
	;LTC("LP9FmsnEooD63juuYKAa3gzeoptXFHFJcB") ;coinbase
	;Sleep 250
	;LTC("MPuhwFaE5PfCxhXTZvuA2aU9wm5gDGDqPb") ;nicehash
	;Sleep 250
	;LTC("MUx1Vs7Qooz6TeSudbUj5Cso1p3mrx3rPm") ;nexo
	;Sleep 250
	LTC("ltc1q3rxefn5keqtdvn9sp52f4rnt8tqewjtjlmdk3z") ;celsius
	Sleep 250
return

#Numpad5::
	XMR("88vNRXQw8sfVpBVK4XPMbUbFT56cceqtGHPrgaLmS6ey37eFFQzn82NfKN9rtg1zuiHJr6RuGHPeWLC63KDMPxEgCqwhsN9") ;nicehash
	Sleep 250	
return

#Numpad6::
	;DASH("XkG2Tu6q7xcB3Lkc4KvPH67idqgQVZqf4W") ;coinbase
	;Sleep 250
	;DASH("7bq2ceYA6JJ2GCJanH2fzKNhEeqer67jie") ;nicehash
	;Sleep 250
	DASH("Xs3SkCVQviQ3cS5EvsB7u7fHSTHmGnSPWX") ;celsius
	Sleep 250
return

#Numpad7::
	;ZEC("t1NLmupUGkGvYekYZfE2uZGHMAnb8irP4tq") ;coinbase
	;Sleep 250
	;ZEC("t3Qu8K36Nga7ADnpD4ZNAroDnG1Dv5MWbo4") ;nicehash
	;Sleep 250
	ZEC("t1W44yMmRF8Wtqu6k8P18JfFDzsAwi1r338") ;celsius
	Sleep 250
return

;------------------------------------------------------------

ETH(cryptoAddress){
	Run, Chrome.exe http://ethereumfaucet.info/,, max
	Sleep 8000
	Send {End}
	Send {PgUp 3}
	Sleep 500
	MouseClick, left, 958, 711
	Sleep 8000
	Send %cryptoAddress%
	Sleep 1000
	Send {Tab}
	Send {Space}
	Sleep 15000
	MouseClick, left, 957, 895
}

ETC(cryptoAddress){
	Run, Chrome.exe http://etcfaucet.info/,, max
	Sleep 8000
	Send {End}
	Send {PgUp 3}
	Sleep 500
	MouseClick, left, 966, 832
	Sleep 8000
	Send %cryptoAddress%
	Sleep 1000
	Send {Tab}
	Send {Space}
	Sleep 15000
	MouseClick, left, 958, 1020
}

XRP(cryptoAddress){
	Run, Chrome.exe http://xrpfaucet.info/,, max
	Sleep 8000
	Send {End}
	Send {PgUp 3}
	Sleep 500
	MouseClick, left, 966, 582
	Sleep 8000
	Send %cryptoAddress%
	Sleep 1000
	Send {Tab 2}
	Sleep 250
	Send {Space}
	Sleep 15000
	MouseClick, left, 958, 1015
}

LTC(cryptoAddress){
	Run, Chrome.exe http://litecoinfaucet.info/,, max
	Sleep 8000
	Send {End}
	Send {PgUp 3}
	Sleep 500
	MouseClick, left, 966, 837
	Sleep 8000
	Send %cryptoAddress%
	Sleep 1000
	Send {Tab}
	Send {Space}
	Sleep 15000
	MouseClick, left, 958, 1026
}

XMR(cryptoAddress){
	Run, Chrome.exe http://monerofaucet.info/,, max
	Sleep 8000
	Send {End}
	Send {PgUp 3}
	Sleep 500
	MouseClick, left, 966, 375
	Sleep 8000
	Send %cryptoAddress%
	Sleep 1000
	Send {Tab}
	Send {Space}
	Sleep 15000
	MouseClick, left, 958, 566
}

DASH(cryptoAddress){
	Run, Chrome.exe http://dashfaucet.net/,, max
	Sleep 8000
	Send {End}
	Send {PgUp 3}
	Sleep 500
	MouseClick, left, 966, 773
	Sleep 8000
	Send %cryptoAddress%
	Sleep 1000
	Send {Tab}
	Send {Space}
	Sleep 15000
	MouseClick, left, 958, 958
}

ZEC(cryptoAddress){
	Run, Chrome.exe http://zcashfaucet.info/,, max
	Sleep 8000
	Send {End}
	Send {PgUp 3}
	Sleep 500
	MouseClick, left, 966, 718
	Sleep 8000
	Send %cryptoAddress%
	Sleep 1000
	Send {Tab}
	Send {Space}
	Sleep 15000
	MouseClick, left, 958, 904
}