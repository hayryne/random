const stream = process.stdin

// read characters instead of binary
stream.setEncoding('utf8')

const hexMap = { A: 10, B: 11, C: 12, D: 13, E: 14, F: 15 }

// converts hexadecimal strings to decimal numbers
// e.g. 2F -> 47
const hexToDec = hex =>
    hex.split('').reverse().reduce((sum, digit, pow) =>
        sum + (hexMap[digit] || digit) * Math.pow(16, pow), 0
    )

const readChar = () => {
  // read a single character from stream
  let char = stream.read(1)

  // nothing read, return null char
  if (!char) return '\0'

  // character is encoded, so read two more
  if (char === '%') {
      char += stream.read(2)

      const num = char.substring(1)

      // if following characters are not valid
      // hex digits, the input is malformed
      if (!/^[0-9a-fA-F]+$/.test(num)) return '\0'

      // convert ascii value to character
      char = String.fromCharCode(hexToDec(num))
  } else if (char === '+') {
      char = ' '
  }

  return char
}

let decodedInput = ''

// this is invoked on every line input
stream.on('readable', () => {
  let char

  do {
    char = readChar()

    decodedInput += char
  } while (char !== '\0')

  // print parsed string
  console.log(decodedInput)

  // and ascii codes of parsed string
  console.log(
    Array(decodedInput.length)
      .fill()
      .map((_, i) => decodedInput.charCodeAt(i))
      .join(' ')
  )
})
