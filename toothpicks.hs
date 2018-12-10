import Graphics.Gloss
import Graphics.Gloss.Interface.Pure.Simulate
import Data.List.Unique

-- particularly inefficient, uses lists

data Alignment = Hor | Ver deriving (Eq, Ord)
data Pick = Pick Point Alignment deriving (Eq, Ord)

type State = ([Pick], [Pick])

delta = 5 -- length of a single toothpick

draw :: State -> Picture
draw (picks, p) = pictures [color red $ line 
    (if a == Ver then [(x, y+delta),(x,y-delta)] else 
     [(x+delta,y), (x-delta,y)]) | Pick (x,y) a <- p]

simulateWorld :: ViewPort -> Float -> State -> State
simulateWorld wp secs (new, old) = (\(n,o) -> (unique n, o)) (foldr f ([], old) new)
    where f n (na,oa) = (add n ++ na, n : oa)
          add p @ (Pick (x,y) a) | a == Ver = Pick (x+delta,y) Hor : [Pick (x-delta,y) Hor]
                                 | a == Hor = Pick (x,y+delta) Ver : [Pick (x,y-delta) Ver]

main = simulate
       (InWindow "toothpicks" (800,800) (0,0)) 
       black 
       10 
       ([Pick (0,0) Ver], []) 
       draw
       simulateWorld