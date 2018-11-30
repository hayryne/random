import System.Environment
import Data.Char
import Data.List hiding (find)
import Data.List.Split

-- assembles symbolic machine language into binary
-- doesn't have much exception handling so don't be a dick with inputs

-- the symbol table, contains tuples of symbols and their values
st = [("SP",0),("LCL",1),("ARG",2),("THIS",3),("THAT",4),("R0",0),
      ("R1",1),("R2",2),("R3",3),("R4",4),("R5",5),("R6",6),("R7",7),
      ("R8",8),("R9",9),("R10",10),("R11",11),("R12",12),("R13",13),
      ("R14",14),("R15",15),("SCREEN",16384), ("KBD",24576)]

main = do
   args <- getArgs
   let path = head args
   let output = head (splitOn ".asm" path) ++ ".hack"
   file <- readFile path
   let ls = parse file
   let pl = labs st ls 0
   let pv = vars pl ls 16
   writeFile output $ unlines $ filter (not . null) $ map (bin pv) ls

-- parses the input file into lines, removing comments and whitespaces
parse :: String -> [String]
parse = filter (not . null) . map clean . lines
   where clean = filter (/= ' ') . head . splitOn "//"

-- parses label contents ie. lines of the form (XXX) to the symbol table
-- takes some symbol table and returns it filled, because Haskell
labs :: [(String,Int)] -> [String] -> Int -> [(String,Int)]
labs sym [] count     = sym
labs sym (x:xs) count | head x == '(' = labs n xs count
                      | otherwise     = labs sym xs $ count+1
   where c = takeWhile (/= ')') $ tail x
         n = (c,count):sym

-- parses variables to the symbol table
vars :: [(String,Int)] -> [String] -> Int -> [(String,Int)]
vars sym [] count     = sym
vars sym (x:xs) count | head x == '@' && l && e = vars n xs $ count+1
                      | otherwise               = vars sym xs count
   where l = isLower $ head $ tail x
         n = (tail x,count):sym
         e = foldr f True sym
            where f a b | tail x /= fst a = b
                        | otherwise       = False

-- assembles a line to binary, implementation varies by line type
bin :: [(String,Int)] -> String -> String
bin sym (x:xs) | x == '@'  = binA sym xs
               | x == '('  = []
               | otherwise = binC (x:xs)

-- assembles commands of type A to binary; knowing that variables may not begin
-- with a digit we can tell symbols from literals using the first character
binA :: [(String,Int)] -> String -> String
binA sym (x:xs) | isDigit x = s (i,16)
                | otherwise = s (l,16)
   where s = concat . map show . toBin
         i = read (x:xs) :: Int
         l = f sym (x:xs)
         f (x:xs) line | line == fst x = snd x
                       | otherwise     = f xs line

-- converts an integer to a binary of a given length
-- example: toBin (15, 8) = [0,0,0,0,1,1,1,1]
toBin :: (Int, Int) -> [Int]
toBin = reverse . unfoldr f
   where f (a,b) | b == 0    = Nothing 
                 | otherwise = Just (rem a 2, (div a 2, b-1))

-- assembles C type commands to binary
binC :: String -> String
binC s = "111" ++ comp s ++ dest s ++ jump s

-- assembles the binary value of a command's dest field
dest :: String -> String
dest s | d == "M"   = "001"
       | d == "D"   = "010"
       | d == "MD"  = "011"
       | d == "A"   = "100"
       | d == "AM"  = "101"
       | d == "AD"  = "110"
       | d == "AMD" = "111"
       | otherwise  = "000"
   where d = takeWhile (/= '=') s

-- assembles the binary value of a command's jump field
jump :: String -> String
jump s | j == "JGT" = "001"
       | j == "JEQ" = "010"
       | j == "JGE" = "011"
       | j == "JLT" = "100"
       | j == "JNE" = "101"
       | j == "JLE" = "110"
       | j == "JMP" = "111"
       | otherwise  = "000"
   where j = drop 1 $ dropWhile (/= ';') s

-- assembles the binary value of a command's comp field
comp :: String -> String
comp s | c == "0"   = "0101010"
       | c == "1"   = "0111111"
       | c == "-1"  = "0111010"
       | c == "D"   = "0001100"
       | c == "A"   = "0110000"
       | c == "M"   = "1110000"
       | c == "!D"  = "0001101"
       | c == "!A"  = "0110011"
       | c == "!M"  = "1110001"
       | c == "-D"  = "0001111"
       | c == "-A"  = "0110011"
       | c == "-M"  = "1110011"
       | c == "D+1" = "0011111"
       | c == "A+1" = "0110111"
       | c == "M+1" = "1110111"
       | c == "D-1" = "0001110"
       | c == "A-1" = "0110010"
       | c == "M-1" = "1110010"
       | c == "D+A" = "0000010"
       | c == "D+M" = "1000010"
       | c == "D-A" = "0010011"
       | c == "D-M" = "1010011"
       | c == "A-D" = "0000111"
       | c == "M-D" = "1000111"
       | c == "D&A" = "0000000"
       | c == "D&M" = "1000000"
       | c == "D|A" = "0010101"
       | c == "D|M" = "1010101"
   where c | elem '=' s = tail $ dropWhile (/= '=') s
           | elem ';' s = takeWhile (/= ';') s