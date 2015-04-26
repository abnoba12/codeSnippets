;Press the windows Button + O to fire this
#o::
loop 1000
{
  x = 1
  y = 1
  Random, x, 0, 800
  Random, y, 0, 740
  MouseMove x, y
  Click
}
return

#0::
loop 10
{
  Send {Del}
  Send {Del}
  Send {Del}
  Send {Del}
  Send {Del}
  Send {Del}
  Send {Del}
  Send {Del}

  Send {Down}  
}

#1::
loop 100
{
  loop 20
  {
    Send {Right}
  }
  Send {Del}
  loop 20
  {
    Send {Right}
  }
  Send {Return}
}