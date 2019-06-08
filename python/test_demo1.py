from hypothesis import given
from hypothesis.strategies import integers 

@given(integers(), integers())
def test_addition_commutative_property(x, y):
    assert x + y == y + x

