namespace ConsoleApp1.PurelyFunctional
{
  class Element<T>
  {
    public T Content { get; set; }
    public Element<T> Next { get; set; }
  }
}
