Example of animation strategies in a windows forms project using the picturebox. 

There are 2 methods used - Events and IObservable (Reactive). Both perform equally well. The reactive implementation syntax is a little cumbersome. 
It doesn't offer many advantages over typical WinForms events in this context. Could be useful in other contexts
because there are OnError and OnCompleted implementations for Observables, where those are not implemented for events.