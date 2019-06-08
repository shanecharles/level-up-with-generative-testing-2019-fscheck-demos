(require '[clojure.test.check :as tc])
(require '[clojure.test.check.generators :as gen])
(require '[clojure.test.check.properties :as prop])

(def addition_commutative_property
  (prop/for-all [v (gen/tuple gen/int gen/int)]
    (let [x (first v)
          y (second v)]
      (= (+ x y) (+ y x)))))

(def addition_commutative_property
  (prop/for-all [v (gen/vector gen/int)]
    (= (reduce + v) (reduce + (reverse v)))))
      
