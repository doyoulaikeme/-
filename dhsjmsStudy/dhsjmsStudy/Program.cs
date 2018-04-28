using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Contexts;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace dhsjmsStudy
{
    #region 简单工厂模式
    public class operation
    {
        public decimal numberA { get; set; }
        public decimal numberB { get; set; }

        public virtual decimal GetResult()
        {
            return 0;
        }
    }


    public class Add : operation
    {
        public override decimal GetResult()
        {
            return numberA + numberB;
        }
    }

    public class Sub : operation
    {
        public override decimal GetResult()
        {
            return numberA - numberB;
        }
    }

    public class Mlu : operation
    {
        public override decimal GetResult()
        {
            return numberA / numberB;
        }
    }

    public class Div : operation
    {
        public override decimal GetResult()
        {
            return numberA * numberB;
        }
    }
    #endregion

    #region 策略模式
    /// <summary>
    /// 抽象收银类
    /// </summary>
    public abstract class CashSuper
    {
        public abstract double acceptCash(double money);
    }
    /// <summary>
    /// 原价
    /// </summary>
    public class CashNormal : CashSuper
    {
        public override double acceptCash(double money)
        {
            return money;
        }
    }

    /// <summary>
    /// 打折
    /// </summary>
    public class CashRebate : CashSuper
    {
        private double moneyRebate = 1d;

        public CashRebate(string moneyRebate)
        {
            this.moneyRebate = double.Parse(moneyRebate);
        }

        public override double acceptCash(double money)
        {
            return money * moneyRebate;
        }
    }

    /// <summary>
    /// 返利
    /// </summary>
    public class CashReturn : CashSuper
    {
        private double moneyCondition = 0;
        private double moneyReturn = 0;

        public CashReturn(string moneyCondition, string moneyReturn)
        {
            this.moneyCondition = double.Parse(moneyCondition);
            this.moneyReturn = double.Parse(moneyReturn);
        }

        public override double acceptCash(double money)
        {
            return money >= moneyCondition ? (money - Math.Floor(money / moneyCondition) * moneyReturn) : money;
        }
    }


    /// <summary>
    /// 策略模式（策略封装了变化）
    /// </summary>
    public class CashContext
    {
        private CashSuper cs;
        public CashContext(CashSuper csuper)
        {
            cs = csuper;
        }

        public CashContext(string type)
        {
            switch (type)
            {
                case "正常消费":
                    cs = new CashNormal();
                    break;
                case "满300返100":
                    cs = new CashReturn("300", "100");
                    break;
                case "打八折":
                    cs = new CashRebate("0.8");
                    break;
            }
        }

        public double GetResult(double money)
        {
            return cs.acceptCash(money);
        }
    }

    #endregion

    #region 装饰模式

    public class Person
    {
        public string name { get; set; }

        public Person()
        {

        }

        public Person(string name)
        {
            this.name = name;
        }

        public virtual void Show()
        {
            Console.WriteLine("装扮的{0}", name);
        }
    }

    /// <summary>
    /// 服饰抽象类
    /// </summary>
    internal class Finery : Person
    {
        protected Person component;


        public override void Show()
        {
            if (component != null)
                component.Show();
        }

        public void Decorate(Person component)
        {
            this.component = component;
        }

    }

    internal class Tshirts : Finery
    {
        public override void Show()
        {
            Console.WriteLine("大T桖");
            base.Show();
        }
    }

    internal class BigTrouser : Finery
    {
        public override void Show()
        {
            Console.WriteLine("垮裤");
            base.Show();
        }
    }
    //其余...
    #endregion

    #region 代理模式

    interface I代理
    {
        void SH();
        void WatchTv();
    }

    public class ZQ : I代理
    {
        private BZQ person;

        public ZQ(BZQ person)
        {
            this.person = person;
        }

        public void SH()
        {
            Console.WriteLine(person.Name + "送花");
        }

        public void WatchTv()
        {
            Console.WriteLine(person.Name + "看电视");
        }
    }

    public class 代理人 : I代理
    {
        private ZQ gg;

        public 代理人(BZQ mm)
        {
            gg = new ZQ(mm);
        }

        public void SH()
        {
            gg.SH();
        }

        public void WatchTv()
        {
            gg.WatchTv();
        }
    }


    public class BZQ
    {
        public string Name { get; set; }
    }

    #endregion

    #region 工厂方法模式

    internal interface I工厂
    {
        operation CreateOperation();
    }

    class AddFactory : I工厂
    {
        public operation CreateOperation()
        {
            return new Add();
        }
    }

    class SubFactory : I工厂
    {
        public operation CreateOperation()
        {
            return new Sub();
        }
    }

    class MulFactory : I工厂
    {
        public operation CreateOperation()
        {
            return new Mlu();
        }
    }

    class DivFactory : I工厂
    {
        public operation CreateOperation()
        {
            return new Div();
        }
    }

    /// <summary>
    /// 雷锋
    /// </summary>

    internal class LeiFeng
    {
        public void Sweep()
        {
            Console.WriteLine("扫地");
        }

        public void Wash()
        {
            Console.WriteLine("洗衣");
        }

        public void BuyRice()
        {
            Console.WriteLine("买米");
        }
    }

    //学雷锋
    internal class StudyPerson : LeiFeng
    {

    }

    class Volunteer : LeiFeng
    {

    }
    //简单雷锋工厂
    internal class SimpleFactory
    {
        public static LeiFeng CreateLeiFeng(string type)
        {
            LeiFeng leiFeng = null;
            switch (type)
            {
                case "大学生":
                    leiFeng = new StudyPerson();
                    break;
                case "社区志愿者":
                    leiFeng = new Volunteer();
                    break;
            }
            return leiFeng;
        }
    }

    interface IFactory
    {
        LeiFeng CreateLeiFeng();
    }

    /// <summary>
    /// 学雷锋的大学生工厂
    /// </summary>
    internal class UndergraduateFactory : IFactory
    {
        public LeiFeng CreateLeiFeng()
        {
            return new StudyPerson();
        }
    }

    internal class VolunteerFactory : IFactory
    {
        public LeiFeng CreateLeiFeng()
        {
            return new Volunteer();
        }
    }

    #endregion

    #region 原型模式
    /// <summary>
    /// 原型类
    /// </summary>
    internal abstract class Prototype
    {
        private string id;
        public string Id
        {
            get { return id; }
        }

        public Prototype(string id)
        {
            this.id = id;
        }

        public abstract Prototype Clone();
    }

    /// <summary>
    /// 具体原型类
    /// </summary>
    internal class ConcretePrototype : Prototype
    {
        public ConcretePrototype(string id)
            : base(id)
        {

        }

        public override Prototype Clone()
        {
            return (Prototype)this.MemberwiseClone();
        }
    }

    /// <summary>
    /// 工作经历类
    /// </summary>
    internal class WorkExperience : ICloneable
    {
        public string wordDate { get; set; }

        public string Company { get; set; }

        public Object Clone()
        {
            return (Object)this.MemberwiseClone();
        }
    }
    /// <summary>
    /// 简历类
    /// </summary>
    internal class Resume : ICloneable
    {
        private string name;
        private string sex;
        private string age;
        //工作经历对象
        private WorkExperience work;

        public Resume(string name)
        {
            this.name = name;
            work = new WorkExperience();
        }

        private Resume(WorkExperience work)
        {
            this.work = (WorkExperience)work.Clone();
        }

        //设置个人信息
        public void SetPersonalInfo(string sex, string age)
        {
            this.sex = sex;
            this.age = age;
        }

        //设置工作经历
        public void SetWorkExperience(string wordDate, string company)
        {
            work.wordDate = wordDate;
            work.Company = company;
        }

        //显示
        public void Display()
        {
            Console.WriteLine("{0},{1},{2}", name, sex, age);
            Console.WriteLine("工作经历：{0},{1}", work.wordDate, work.Company);
        }

        public Object Clone()
        {
            Resume obj = new Resume(this.work);
            obj.name = this.name;
            obj.sex = this.sex;
            obj.age = this.age;
            return obj;
        }
    }

    #endregion

    #region 模板方法模式

    internal class TestPaper
    {
        public void TestQuestion1()
        {
            Console.WriteLine("第一题：a b c d ");
            Console.WriteLine(Answer1());
        }

        public void TestQuestion2()
        {
            Console.WriteLine("第二题：a b c d ");
            Console.WriteLine(Answer2());
        }

        public void TestQuestion3()
        {
            Console.WriteLine("第三题：a b c d ");
            Console.WriteLine(Answer3());
        }

        protected virtual string Answer1()
        {
            return "";
        }
        protected virtual string Answer2()
        {
            return "";
        }
        protected virtual string Answer3()
        {
            return "";
        }
    }

    internal class TestPaperA : TestPaper
    {
        protected override string Answer1()
        {
            return "a";
        }
        protected override string Answer2()
        {
            return "b";
        }
        protected override string Answer3()
        {
            return "c";
        }

    }


    internal class TestPaperB : TestPaper
    {
        protected override string Answer1()
        {
            return "c";
        }
        protected override string Answer2()
        {
            return "b";
        }
        protected override string Answer3()
        {
            return "a";
        }
    }

    #endregion

    #region 外观模式
    /// <summary>
    /// 股票1
    /// </summary>
    internal class Stock1
    {
        public void Sell()
        {
            Console.WriteLine("1卖出");
        }

        public void Buy()
        {
            Console.WriteLine("1买入");
        }
    }

    /// <summary>
    /// 股票2
    /// </summary>
    internal class Stock2
    {
        public void Sell()
        {
            Console.WriteLine("2卖出");
        }

        public void Buy()
        {
            Console.WriteLine("2买入");
        }
    }

    /// <summary>
    /// 投资基金类
    /// </summary>
    internal class Fund
    {
        private Stock1 gu1;
        private Stock2 gu2;

        public Fund()
        {
            gu1 = new Stock1();
            gu2 = new Stock2();
        }


        public void BuyFund()
        {
            gu1.Buy();
            gu2.Buy();
        }

        public void SellFund()
        {
            gu1.Sell();
            gu2.Sell();
        }


    }

    #endregion

    #region 建造者模式

    public class Product
    {
        IList<string> parts = new List<string>();

        /// <summary>
        /// 添加产品部件
        /// </summary>
        /// <param name="part"></param>
        public void Add(string part)
        {
            parts.Add(part);
        }

        /// <summary>
        /// 列举所有的产品部件
        /// </summary>
        public void Show()
        {
            Console.WriteLine("产品 创建 ----");
            foreach (var part in parts)
            {
                Console.WriteLine(part);
            }
        }
    }

    /// <summary>
    /// 抽象建造者类
    /// </summary>
    public abstract class Builder
    {
        public abstract void BuildePartA();

        public abstract void BuildePartB();

        public abstract Product GetResult();
    }

    public class ConcreteBuilder1 : Builder
    {
        private Product product = new Product();

        public override void BuildePartA()
        {
            product.Add("部件a");
        }

        public override void BuildePartB()
        {
            product.Add("部件b");
        }

        public override Product GetResult()
        {
            return product;
        }
    }

    public class ConcreteBuilder2 : Builder
    {
        private Product product = new Product();

        public override void BuildePartA()
        {
            product.Add("部件XX");
        }

        public override void BuildePartB()
        {
            product.Add("部件YY");
        }

        public override Product GetResult()
        {
            return product;
        }
    }

    internal class Director
    {
        public void Construct(Builder builder)
        {
            builder.BuildePartA();
            builder.BuildePartB();
        }
    }

    #endregion

    #region 观察者模式

    #region 最终版

    public delegate void EventHandler();
    /// <summary>
    /// 通知者接口
    /// </summary>
    public interface ISubject
    {
        void Attach(Observer observer);

        void Detach(Observer observer);

        void Notify();

        string SubjectState { get; set; }
    }

    public class Boss : ISubject
    {
        private IList<Observer> observers = new List<Observer>();
        private string action;

        public event EventHandler Update;
        //老板状态
        public string SubAction
        {
            get { return action; }
            set { action = value; }
        }
        /// <summary>
        /// 添加要帮忙的同事
        /// </summary>
        /// <param name="observer"></param>
        public void Attach(Observer observer)
        {
            observers.Add(observer);
        }

        public void Detach(Observer observer)
        {
            observers.Remove(observer);
        }

        public void Notify1()
        {
            Update();
        }
        //通知
        public void Notify()
        {
            foreach (Observer o in observers)
            {
                o.Update();
            }
        }
        public string SubjectState { get; set; }
    }
    public abstract class Observer
    {
        protected string name;
        protected ISubject sub;

        public Observer(string name, ISubject sub)
        {
            this.name = name;
            this.sub = sub;
        }

        public abstract void Update();
    }

    /// <summary>
    /// 看股票的
    /// </summary>
    public class StockObserver : Observer
    {
        public StockObserver(string name, ISubject sub)
            : base(name, sub)
        {
        }

        public override void Update()
        {
            Console.WriteLine("{0} {1} 关闭股票咯，继续工作。", sub.SubjectState, name);
        }

        //事件委托实现
        public void CloseStockMarket()
        {
            Console.WriteLine("{0} {1} 关闭股票咯，继续工作。委托", sub.SubjectState, name);
        }
    }


    /// <summary>
    /// 看NBA的
    /// </summary>
    public class NBAObserver : Observer
    {
        public NBAObserver(string name, ISubject sub)
            : base(name, sub)
        {
        }

        public override void Update()
        {
            Console.WriteLine("{0} {1} 关闭NBA咯，继续工作。", sub.SubjectState, name);
        }


        //事件委托实现
        public void CloseNBA()
        {
            Console.WriteLine("{0} {1} 关闭NBA咯，继续工作。委托", sub.SubjectState, name);
        }
    }
    #endregion



    public class Secretary
    {
        private IList<StockObserver> observers = new List<StockObserver>();
        private string action;

        /// <summary>
        /// 添加要帮忙的同事
        /// </summary>
        /// <param name="observer"></param>
        public void Attach(StockObserver observer)
        {
            observers.Add(observer);
        }

        public void Detach(StockObserver observer)
        {
            observers.Remove(observer);
        }


        //通知
        public void Notify()
        {
            foreach (StockObserver o in observers)
            {
                o.Update();
            }
        }

        //前台状态
        public string SecretaryAction
        {
            get { return action; }
            set { action = value; }
        }
    }

    #endregion

    #region 抽象工厂模式

    public class User
    {
        public int id { get; set; }

        public string name { get; set; }

    }

    public interface IUser
    {
        void Insert(User user);

        User GetUser(int id);
    }

    public class SqlserverUser : IUser
    {
        public void Insert(User user)
        {
            Console.WriteLine("Sqlserver添加一条消息");
        }

        public User GetUser(int id)
        {
            Console.WriteLine("Sqlserver查询一条消息");
            return null;
        }
    }

    public class AccessUser : IUser
    {
        public void Insert(User user)
        {
            Console.WriteLine("Access添加一条消息");
        }

        public User GetUser(int id)
        {
            Console.WriteLine("Access查询一条消息");
            return null;
        }
    }

    public interface IFactoryDB
    {
        IUser CreateUser();
    }

    public class SqlServerFactory : IFactoryDB
    {
        public IUser CreateUser()
        {
            return new SqlserverUser();
        }
    }

    public class AccessFactory : IFactoryDB
    {
        public IUser CreateUser()
        {
            return new AccessUser();
        }
    }

    //用简单工厂改进抽象工厂
    //public class DataAccess
    //{
    //    private static readonly string db = "Sqlserver";
    //    //private static readonly string db = "Access";

    //    public static Iuser
    //}

    #endregion

    #region 状态模式

    public abstract class State
    {
        public abstract void Handle(Context context);
    }

    public class ConcreteStateA : State
    {
        public override void Handle(Context context)
        {
            context.State = new ConcreteStateB();
        }
    }

    public class ConcreteStateB : State
    {
        public override void Handle(Context context)
        {
            context.State = new ConcreteStateA();
        }
    }

    public class Context
    {
        private State state;

        public Context(State state)
        {
            this.state = state;
        }

        public State State
        {
            get { return state; }
            set
            {
                state = value;
                Console.WriteLine("当前状态：" + state.GetType().Name);
            }
        }

        public void Request()
        {
            state.Handle(this);
        }
    }

    #endregion

    #region 适配器模式

    public class Target
    {
        public virtual void Request()
        {
            Console.WriteLine("普通请求！");
        }
    }

    public class Adaptee
    {
        public void SpecificRequest()
        {
            Console.WriteLine("特殊请求！");
        }
    }

    public class Adapter : Target
    {
        private Adaptee adaptee = new Adaptee();

        public override void Request()
        {
            adaptee.SpecificRequest();
        }
    }

    #endregion

    #region 备忘录模式
    /// <summary>
    /// 发起人
    /// </summary>
    public class Originator
    {
        public string state { get; set; }

        public Memento CreateMemento()
        {
            return new Memento(state);
        }

        public void SetMemento(Memento memento)
        {
            state = memento.State;
        }

        public void Show()
        {
            Console.WriteLine("State=" + state);
        }
    }

    /// <summary>
    /// 备忘类
    /// </summary>
    public class Memento
    {
        private string state;

        public Memento(string state)
        {
            this.state = state;
        }

        public string State
        {
            get { return state; }
        }
    }
    /// <summary>
    /// 管理者
    /// </summary>
    public class Caretaker
    {
        public Memento memento { get; set; }
    }

    #endregion

    #region 组合模式

    public abstract class Component
    {
        protected string name;

        public Component(string name)
        {
            this.name = name;
        }

        public abstract void Add(Component c);
        public abstract void Remove(Component c);
        public abstract void Display(int depth);

    }

    public class Leaf : Component
    {
        public Leaf(string name)
            : base(name)
        {
        }

        public override void Add(Component c)
        {
            Console.WriteLine("添加");
        }

        public override void Remove(Component c)
        {
            Console.WriteLine("删除");
        }

        public override void Display(int depth)
        {
            Console.WriteLine(new string('-', depth) + name);
        }
    }

    public class Composite : Component
    {
        private List<Component> children = new List<Component>();
        public Composite(string name)
            : base(name)
        {
        }

        public override void Add(Component c)
        {
            children.Add(c);
        }

        public override void Remove(Component c)
        {
            children.Remove(c);
        }

        public override void Display(int depth)
        {
            Console.WriteLine(new string('-', depth) + name);

            foreach (Component component in children)
            {
                component.Display(depth + 2);
            }
        }
    }

    #endregion

    #region 迭代器模式

    /// <summary>
    /// Iterator迭代器抽象类
    /// </summary>
    public abstract class Iterator
    {
        public abstract object First();

        public abstract object Next();

        public abstract bool IsDone();
        public abstract object CurrentItem();
    }

    /// <summary>
    /// 聚集抽象类
    /// </summary>
    public abstract class Aggregate
    {
        public abstract Iterator CreateIterator();
    }


    public class ConcreteIterator : Iterator
    {
        private ConcreteAggregate aggregate;
        private int current = 0;

        public ConcreteIterator(ConcreteAggregate aggregate)
        {
            this.aggregate = aggregate;
        }

        public override object First()
        {
            return aggregate[0];
        }

        public override object Next()
        {
            object ret = null;
            current++;
            if (current < aggregate.Count)
                ret = aggregate[current];
            return ret;
        }

        public override bool IsDone()
        {
            return current >= aggregate.Count;
        }

        public override object CurrentItem()
        {
            return aggregate[current];
        }
    }


    public class ConcreteAggregate : Aggregate
    {
        private IList<object> items = new List<object>();
        public override Iterator CreateIterator()
        {
            return new ConcreteIterator(this);
        }

        public int Count
        {
            get { return items.Count; }
        }

        public object this[int index]
        {
            get { return items[index]; }
            set { items.Insert(index, value); }
        }
    }

    #endregion

    #region 单例模式

    public class Singleton
    {
        public static Singleton instance;

        private static readonly object syncRoot = new object();
        private Singleton()
        {

        }

        public static Singleton GetInstance()
        {
            //双重锁定
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new Singleton();
                    }
                }
            }
            return instance;
        }
    }

    #endregion

    #region 桥接模式

    //public partial class HandsetGame
    //{
    //    public virtual void Run()
    //    {

    //    }
    //}

    //public class HandsetMGame : HandsetGame
    //{
    //    public override void Run()
    //    {
    //        Console.WriteLine("运行M");
    //    }
    //}

    //public class HandsetNGame : HandsetGame
    //{
    //    public override void Run()
    //    {
    //        Console.WriteLine("运行N");
    //    }
    //}


    #region 松耦合的程序
    /// <summary>
    /// 手机软件
    /// </summary>
    public abstract class HandsetSoft
    {
        public abstract void Run();
    }
    /// <summary>
    /// 手机游戏
    /// </summary>
    public class HandsetGame : HandsetSoft
    {
        public override void Run()
        {
            Console.WriteLine("运行手机游戏");
        }
    }
    /// <summary>
    /// 手机通讯录
    /// </summary>
    public class HandsetAddressList : HandsetSoft
    {
        public override void Run()
        {
            Console.WriteLine("运行手机通讯录");
        }
    }

    /// <summary>
    /// 手机品牌类
    /// </summary>
    public abstract class HandsetBrand
    {
        protected HandsetSoft soft;

        public void SetHandsetSoft(HandsetSoft soft)
        {
            this.soft = soft;
        }

        public abstract void Run();
    }

    public class HandsetBrandM : HandsetBrand
    {
        public override void Run()
        {
            soft.Run();
        }
    }
    public class HandsetBrandN : HandsetBrand
    {
        public override void Run()
        {
            soft.Run();
        }
    }
    #endregion
    #endregion

    #region 命令模式
    /// <summary>
    /// 抽象命令
    /// </summary>
    public abstract class Command
    {
        protected Barbecuer receiver;

        public Command(Barbecuer receiver)
        {
            this.receiver = receiver;
        }

        public abstract void ExcuteCommand();
    }


    public class Barbecuer
    {
        public void BakeMutton()
        {
            Console.WriteLine("烤肉");
        }

        public void BakeChickenWing()
        {
            Console.WriteLine("鸡翅");
        }
    }

    /// <summary>
    /// 烤肉
    /// </summary>
    public class BakeMuttonCommand : Command
    {
        public BakeMuttonCommand(Barbecuer receiver)
            : base(receiver)
        {
        }

        public override void ExcuteCommand()
        {
            receiver.BakeMutton();
        }
    }

    /// <summary>
    /// 烤鸡翅
    /// </summary>
    public class BakeChickenWingCommand : Command
    {
        public BakeChickenWingCommand(Barbecuer receiver)
            : base(receiver)
        {
        }

        public override void ExcuteCommand()
        {
            receiver.BakeChickenWing();
        }
    }

    /// <summary>
    /// 服务员
    /// </summary>
    public class Waiter
    {
        private IList<Command> orders = new List<Command>();

        public void SetOrder(Command command)
        {
            if (command.ToString() == "dhsjmsStudy.BakeChickenWingCommand")
            {
                Console.WriteLine("鸡翅卖完了！");
            }
            else
            {
                orders.Add(command);
                Console.WriteLine("添加订单：" + command + " 时间：" + DateTime.Now);
            }
        }

        public void CancelOrder(Command command)
        {
            orders.Remove(command);
            Console.WriteLine("取消订单：" + command + " 时间：" + DateTime.Now);
        }

        public void Notify()
        {
            foreach (Command command in orders)
            {
                command.ExcuteCommand();
            }
        }
    }

    #endregion

    #region 责任链模式

    public class Request
    {
        public string requestType { get; set; }
        public string requestContent { get; set; }
        public int numBer { get; set; }
    }

    /// <summary>
    /// 管理者
    /// </summary>
    public abstract class Manager
    {
        protected string name;

        //管理者的上级
        protected Manager superior;

        public Manager(string name)
        {
            this.name = name;
        }

        //设置管理者的上级
        public void SetSuperior(Manager superior)
        {
            this.superior = superior;
        }

        public abstract void RequestApplications(Request request);
    }

    /// <summary>
    /// 经理
    /// </summary>
    public class CommonManager : Manager
    {
        public CommonManager(string name)
            : base(name)
        {
        }

        public override void RequestApplications(Request request)
        {
            if (request.requestType == "请假" && request.numBer <= 2)
            {
                Console.WriteLine("{0}:{1} 数量{2} 被批准", name, request.requestContent, request.numBer);
            }
            else
            {
                if (null != superior)
                    superior.RequestApplications(request);
            }
        }
    }

    /// <summary>
    /// 总监
    /// </summary>
    public class Majordomo : Manager
    {
        public Majordomo(string name)
            : base(name)
        {
        }

        public override void RequestApplications(Request request)
        {
            if (request.requestType == "请假" && request.numBer <= 5)
            {
                Console.WriteLine("{0}:{1} 数量{2} 被批准", name, request.requestContent, request.numBer);
            }
            else
            {
                if (null != superior)
                    superior.RequestApplications(request);
            }
        }
    }


    /// <summary>
    /// 总经理
    /// </summary>
    public class GeneralManager : Manager
    {
        public GeneralManager(string name)
            : base(name)
        {
        }

        public override void RequestApplications(Request request)
        {
            if (request.requestType == "请假")
            {
                Console.WriteLine("{0}:{1} 数量{2} 被批准", name, request.requestContent, request.numBer);
            }
            else if (request.requestType == "加薪" && request.numBer <= 500)
            {
                Console.WriteLine("{0}:{1} 数量{2} 被批准", name, request.requestContent, request.numBer);
            }
            else if (request.requestType == "加薪" && request.numBer > 500)
            {
                Console.WriteLine("{0}:{1} 数量{2} 再说吧", name, request.requestContent, request.numBer);
            }

        }
    }
    #endregion

    #region 中介者模式
    /// <summary>
    /// 抽象中介类
    /// </summary>
    public abstract class Mediator
    {
        public abstract void Send(string message, Colleague colleague);
    }

    public class ConcreteMediator : Mediator
    {
        private ConcreteMediator1 col1;
        private ConcreteMediator2 col2;

        public ConcreteMediator1 Col1
        {
            set { col1 = value; }
        }

        public ConcreteMediator2 Col2
        {
            set { col2 = value; }
        }
        public override void Send(string message, Colleague colleague)
        {
            if (colleague == col1)
            {
                col2.Notify(message);
            }
            else
            {
                col1.Notify(message);
            }
        }
    }

    /// <summary>
    /// 抽象同事类
    /// </summary>
    public abstract class Colleague
    {
        protected Mediator mediator;

        public Colleague(Mediator mediator)
        {
            this.mediator = mediator;
        }
    }
    public class ConcreteMediator1 : Colleague
    {
        public ConcreteMediator1(Mediator mediator)
            : base(mediator)
        {
        }

        public void Send(string message)
        {
            mediator.Send(message, this);
        }

        public void Notify(string message)
        {
            Console.WriteLine("同事1得到的消息：" + message);
        }
    }

    public class ConcreteMediator2 : Colleague
    {
        public ConcreteMediator2(Mediator mediator)
            : base(mediator)
        {
        }

        public void Send(string message)
        {
            mediator.Send(message, this);
        }

        public void Notify(string message)
        {
            Console.WriteLine("同事2得到的消息：" + message);
        }
    }
    #endregion

    #region 享元模式

    public abstract class Flyweight
    {
        public abstract void Operation(int extrinsicstate);
    }

    public class ConcreteFlyweight : Flyweight
    {
        public override void Operation(int extrinsicstate)
        {
            Console.WriteLine("具体Flyweight:" + extrinsicstate);
        }
    }

    public class UnshareConcreteFlyweight : Flyweight
    {
        public override void Operation(int extrinsicstate)
        {
            Console.WriteLine("不共享的具体Flyweight:" + extrinsicstate);
        }
    }

    public class FlyweightFactory
    {
        private Hashtable flyweights = new Hashtable();

        public FlyweightFactory()
        {
            flyweights.Add("X", new ConcreteFlyweight());
            flyweights.Add("Y", new ConcreteFlyweight());
            flyweights.Add("Z", new ConcreteFlyweight());
        }

        public Flyweight GetFlyweight(string key)
        {
            return ((Flyweight)flyweights[key]);
        }
    }
    #region 网站享元

    public abstract class WebSite
    {
        public abstract void Use();
    }

    /// <summary>
    /// 具体网站
    /// </summary>
    public class ConcreteWebSite : WebSite
    {
        private string _name;

        public ConcreteWebSite(string name)
        {
            this._name = name;
        }

        public override void Use()
        {
            Console.WriteLine("网站分类：" + _name);
        }
    }

    /// <summary>
    /// 网站工厂类
    /// </summary>
    public class WebSiteFactory
    {
        private Hashtable flyweights = new Hashtable();

        //获取网站分类
        public WebSite GetWebSiteCategory(string key)
        {
            if (!flyweights.ContainsKey(key))
                flyweights.Add(key, new ConcreteWebSite(key));
            return ((WebSite)flyweights[key]);
        }

        public int GetWebSiteCount()
        {
            return flyweights.Count;
        }
    }

    #endregion
    #endregion

    #region 解释器模式

    public class Context1
    {
        public string inPut { get; set; }

        public string outPut { get; set; }
    }

    public class TerminalExpression : AbstractExpression
    {
        public override void Interpret(Context1 context)
        {
            Console.WriteLine("终端解释器");
        }
    }

    public abstract class AbstractExpression
    {
        public abstract void Interpret(Context1 context);
    }

    public class NoterminalExpression : AbstractExpression
    {
        public override void Interpret(Context1 context)
        {
            Console.WriteLine("非终端解释器");
        }
    }


    #endregion

    #region 访问者模式

    public abstract class Visitor
    {
        public abstract void VisitorConcreteElementA(ConcreteElementA A);
        public abstract void VisitorConcreteElementB(ConcreteElementB B);
    }

    public class ConcreteVisitorA : Visitor
    {
        public override void VisitorConcreteElementA(ConcreteElementA A)
        {
            Console.WriteLine("{0}被{1}访问", A.GetType().Name, this.GetType().Name);
        }

        public override void VisitorConcreteElementB(ConcreteElementB B)
        {
            Console.WriteLine("{0}被{1}访问", B.GetType().Name, this.GetType().Name);
        }
    }

    public class ConcreteVisitorB : Visitor
    {
        public override void VisitorConcreteElementA(ConcreteElementA A)
        {
            Console.WriteLine("{0}被{1}访问", A.GetType().Name, this.GetType().Name);
        }

        public override void VisitorConcreteElementB(ConcreteElementB B)
        {
            Console.WriteLine("{0}被{1}访问", B.GetType().Name, this.GetType().Name);
        }
    }

    public abstract class Element
    {
        public abstract void Accept(Visitor visitor);
    }

    public class ConcreteElementA : Element
    {
        public override void Accept(Visitor visitor)
        {
            visitor.VisitorConcreteElementA(this);
        }

        public void OperationA()
        {

        }
    }

    public class ConcreteElementB : Element
    {
        public override void Accept(Visitor visitor)
        {
            visitor.VisitorConcreteElementB(this);
        }

        public void OperationB()
        {

        }
    }

    /// <summary>
    /// 提供高层接口，允许访问者访问
    /// </summary>
    public class ObjectStructure
    {
        private IList<Element> elements = new List<Element>();

        public void Attach(Element element)
        {
            elements.Add(element);
        }

        public void Detach(Element element)
        {
            elements.Remove(element);
        }

        public void Accept(Visitor visitor)
        {
            foreach (Element e in elements)
            {
                e.Accept(visitor);
            }
        }
    }

    #endregion
    class Program
    {

        public static operation CreateOperate(string operate)
        {
            operation oper = null;
            switch (operate)
            {
                case "+":
                    oper = new Add();
                    break;
                case "-":
                    oper = new Sub();
                    break;
                case "*":
                    oper = new Div();
                    break;
                case "/":
                    oper = new Mlu();
                    break;
            }
            return oper;
        }

        public static CashSuper CreateCashAccept(string type)
        {
            CashSuper cs = null;
            switch (type)
            {
                case "正常消费":
                    cs = new CashNormal();
                    break;
                case "满300返100":
                    cs = new CashReturn("300", "100");
                    break;
                case "打八折":
                    cs = new CashRebate("0.8");
                    break;
            }
            return cs;
        }

        static void Main(string[] args)
        {
            #region 简单工厂模式
            //operation oper = Program.CreateOperate("*");
            //oper.numberB = 10;
            //oper.numberA = 11;
            //Console.WriteLine(oper.GetResult());
            #endregion

            #region 策略模式与简单工厂组合
            //CashContext cc = new CashContext("满300返100");
            //double totalPrices = cc.GetResult(10000);
            //Console.WriteLine(totalPrices);
            #endregion

            #region 单一原则
            //如果一个类承担的职责过多，就等于把这些职责耦合在一起。
            //一个职责的变化可能会削弱或者抑制这个类完成其他职责的能力。
            //这种耦合会导致脆弱的设计，当发生变化时，设计会遭受意想不到的破坏【ASD】。

            //软件设计真正要做的许多内容，就是发现职责并相互分离。
            //如果你能够想到多于一个的动机去改变一个类，那么这个类就具备多于一个的职责，
            //就应该考虑类的职责分离。
            #endregion

            #region 开放-封闭原则
            //(大话代码结构)将BLL的业务逻辑开放，将修改等数据处理封闭。

            //(大话设计模式)软件实体（类、模块、函数等等）应该可以扩展，但是不可修改。
            //对于扩展是开放的，对于更改是封闭的。

            //对于需求、程序改动是通过增加新代码进行的，而不是更改现有代码。
            #endregion

            #region 依赖倒转原则
            //针对于接口编程，不要对实现编程。

            //A.高层模块不应该依赖底层模块。两个都依赖抽象。
            //B.抽象不应依赖细节，细节应依赖抽象。
            #endregion

            #region 里氏代换原则
            //子类型必须能够替换它们的父类型。
            #endregion

            #region 装饰模式
            //动态地给一个对象添加一些额外的职能，
            //就增加功能来说，装饰模式比生成子类更灵活

            //Person ly = new Person("蝼蚁");
            //Console.WriteLine("第一种装扮：");
            //Finery dtx = new Tshirts();
            //Finery kk = new BigTrouser();
            //dtx.Decorate(ly);
            //kk.Decorate(dtx);
            //kk.Show();
            #endregion

            #region 代理模式
            ////为其他对象提供一种代理以控制对这个对象的访问
            //BZQ jiaojiao = new BZQ();
            //jiaojiao.Name = "娇娇";

            //代理人 daili = new 代理人(jiaojiao);
            //daili.SH();
            //daili.WatchTv();
            #endregion

            #region 工厂方法模式
            //I工厂 operFactory = new AddFactory();
            //operation oper = operFactory.CreateOperation();
            //oper.numberA = 1;
            //oper.numberB = 2;
            //Console.WriteLine(oper.GetResult());

            ////大学生和志愿者可以通过实例化不同类实现。
            ////IFactory factory= new UndergraduateFactory();
            //IFactory factory = new VolunteerFactory();
            //LeiFeng leifeng = factory.CreateLeiFeng();
            //leifeng.BuyRice();
            //leifeng.Sweep();
            //leifeng.Wash();

            #endregion

            #region 原型模式
            ////用原型实例指定创建对象的种类，
            ////并通过拷贝这些原型创建新的对象。

            ////ConcretePrototype pl = new ConcretePrototype("I");
            ////ConcretePrototype cl = (ConcretePrototype)pl.Clone();

            ////Console.WriteLine("Clone:{0}", cl.Id);
            ////Clone复制结构不复制数据
            ////Copy复制结构也复制数据

            //Resume a = new Resume("test");
            //a.SetPersonalInfo("男", "22");
            //a.SetWorkExperience("1995-2018", "xx公司");

            ////此处Clone()=>实则是在重新实例化一个Resume再赋值
            //Resume b = (Resume)a.Clone();
            //b.SetWorkExperience("1998-2006", "yy公司");

            //Resume c = (Resume)a.Clone();
            //c.SetWorkExperience("1998-2006", "zz公司");

            //a.Display();
            //b.Display();
            //c.Display();
            #endregion

            #region 模板方法模式
            ////定义一个操作中的算法的骨架，
            ////而将一些步骤延迟到子类中。
            ////使得子类可以不改变一个算法的结构
            ////即可重定义该算法的某些特定步骤。
            //Console.WriteLine("考试A");
            //TestPaper studentA = new TestPaperA();
            //studentA.TestQuestion1();
            //studentA.TestQuestion2();
            //studentA.TestQuestion3();

            //Console.WriteLine("考试B");
            //TestPaper studentB = new TestPaperB();
            //studentB.TestQuestion1();
            //studentB.TestQuestion2();
            //studentB.TestQuestion3();

            #endregion

            #region 迪米特法则
            //如果两个类不必彼此直接通信，
            //那么这两个类就不应当发生直接的相互作用。
            //如果其中一个类需要调用另一个类的某一个方法的话，
            //可以通过第三者转发这个调用。

            //如果再公司电脑坏了，可以寻求该IT部门，而不是直接找某个人。
            #endregion

            #region 外观模式
            ////为子系统中的一组接口提供一个一致的界面，
            ////此模式定义了一个高层接口，使得子系统更容易使用。
            ////遇到复杂庞大的系统维护时，我们可添加一个简单的接口，
            ////减少它们之间的依赖。

            ////Stock1 gu1 = new Stock1();
            ////Stock2 gu2 = new Stock2();

            ////gu1.Buy();
            ////gu2.Buy();

            ////gu1.Sell();
            ////gu2.Sell();

            //Fund jijin = new Fund();
            //jijin.BuyFund();
            //jijin.SellFund();
            #endregion

            #region 建造者模式
            ////将一个复杂对象的构造与它的表示分离,
            ////使得同样的构造过程可以创建不同的表示。

            //Director director = new Director();
            //Builder b1 = new ConcreteBuilder1();
            //Builder b2 = new ConcreteBuilder2();

            //director.Construct(b1);
            //Product p1 = b1.GetResult();
            //p1.Show();

            //director.Construct(b2);
            //Product p2 = b2.GetResult();
            //p2.Show();
            #endregion

            #region 观察者模式
            ////定义一种一对多的依赖关系,
            ////让多个观察者对象同时监听某一个主题对象。
            ////当这个主题对象变化时，会通知所有观察对象，
            ////使它们能自动更新自己。

            //Secretary qiantai = new Secretary();
            //StockObserver tongzhi1 = new StockObserver("通知人1", qiantai);
            //StockObserver tongzhi2 = new StockObserver("通知人2", qiantai);

            //qiantai.Attach(tongzhi1);
            //qiantai.Attach(tongzhi2);

            //qiantai.SecretaryAction = "老板来了！";
            //qiantai.Notify();

            #region 最终版
            //Boss huhansan = new Boss();

            //StockObserver tongzhi1 = new StockObserver("通知人1", huhansan);
            //NBAObserver tongzhi2 = new NBAObserver("通知人2", huhansan);

            //huhansan.Attach(tongzhi1);
            //huhansan.Attach(tongzhi2);

            //huhansan.Detach(tongzhi1);

            //huhansan.SubjectState = "我胡汉三又回来啦";

            //huhansan.Notify();
            #endregion

            #region 事件委托
            //Boss huhansan = new Boss();

            //StockObserver tongzhi1 = new StockObserver("通知人1", huhansan);
            //NBAObserver tongzhi2 = new NBAObserver("通知人2", huhansan);
            //huhansan.Update += new EventHandler(tongzhi1.CloseStockMarket);//应该是把tongzhi1对象给一起传入方法了。
            //huhansan.Update += new EventHandler(tongzhi2.CloseNBA);
            //huhansan.SubjectState = "我胡汉三又回来啦";
            //huhansan.Notify1();
            #endregion
            #endregion

            #region 抽象工厂模式
            //提供一个创建一系列相关或相互依赖对象的接口,
            //而无需指定它们的具体类。

            //User user = new User();
            //IFactoryDB factory = new SqlServerFactory();
            //IUser iu = factory.CreateUser();
            //iu.Insert(user);
            //iu.GetUser(1);

            //用反射+抽象工厂的数据访问程序

            #endregion

            #region 状态模式
            ////当一个对象的内在状态改变时允许改变其行为，
            ////这个对象看起来像是改变了其类。

            //Context c = new Context(new ConcreteStateA());
            //c.Request();
            //c.Request();
            //c.Request();
            //c.Request();
            #endregion

            #region 适配器模式
            //将一个类的接口转换成客户希望的另一个接口,
            //Adapter模式使得原本由于接口不兼容而不能一起工作的类，
            //可以一起工作。

            //Target target = new Adapter();
            //target.Request();
            #endregion

            #region 备忘录模式
            ////在不破坏封装性的前提下，
            ////捕获一个对象的内部状态，
            ////并在该对象之外保存这个状态。
            //Originator o = new Originator();
            //o.state = "On";
            //o.Show();

            //Caretaker c = new Caretaker();
            //c.memento = o.CreateMemento();

            //o.state = "off";
            //o.Show();

            //o.SetMemento(c.memento);
            //o.Show();

            #endregion

            #region 组合模式
            ////将对象组合成树形结构以表示“部分-整体”的层次结构。
            ////组合模式使得用户对单个对象和组合对象的使用具有一致性。

            ////生成根节点
            //Composite root = new Composite("root");
            //root.Add(new Leaf("Leaf A"));
            //root.Add(new Leaf("Leaf B"));

            ////根上长出分支
            //Composite comp = new Composite("Composite X");
            //comp.Add(new Leaf("Leaf XA"));
            //comp.Add(new Leaf("Leaf XB"));

            //root.Add(comp);

            //Composite comp2 = new Composite("Composite XY");
            //comp.Add(new Leaf("Leaf XYA"));
            //comp.Add(new Leaf("Leaf XYB"));

            //comp.Add(comp2);
            //root.Add(new Leaf("Leaf C"));

            //Leaf leaf = new Leaf("Leaf D");
            //root.Add(leaf);
            //root.Remove(leaf);

            ////显示大树
            //root.Display(1);
            #endregion

            #region 迭代器模式
            //ConcreteAggregate a = new ConcreteAggregate();
            //for (int i = 0; i < 10; i++)
            //{
            //    a[i] = i;
            //}

            //Iterator i1 = new ConcreteIterator(a);
            //while (!i1.IsDone())
            //{
            //    Console.WriteLine("{0} 请买票!", i1.CurrentItem());
            //    i1.Next();
            //}
            #endregion

            #region 单例模式
            ////保证一个类仅有一个实例，
            ////并提供一个访问它的全局访问点。

            //Singleton s1 = Singleton.GetInstance();
            //Singleton s2 = Singleton.GetInstance();

            //if (s1 == s2)
            //    Console.WriteLine("相同实例");


            #endregion

            #region 桥接模式
            //将抽象部分与它的实现部分分离，
            //使它们都可以独立变化。

            //同样一个东西，运行在不同平台。
            //只要把东西弄好，其他平台继承后实现父类即可。

            ////HandsetGame a;
            ////a = new HandsetMGame();
            ////a.Run();

            ////a= new HandsetNGame();
            ////a.Run();

            //HandsetBrand hb;
            //hb = new HandsetBrandM();

            //hb.SetHandsetSoft(new HandsetGame());
            //hb.Run();

            //hb.SetHandsetSoft(new HandsetAddressList());
            //hb.Run();

            //hb = new HandsetBrandN();

            //hb.SetHandsetSoft(new HandsetGame());
            //hb.Run();

            //hb.SetHandsetSoft(new HandsetAddressList());
            //hb.Run();

            #endregion

            #region 合成/聚合复用原则
            //尽量使用合成/聚合，
            //尽量不要使用类继承
            #endregion

            #region 命令模式
            ////将一个请求封装为一个对象，
            ////从而使你可用不同的请求对客户进行参数化;
            ////对请求排队或记录请求日志，以及支持可撤销的操作。
            //Barbecuer boy = new Barbecuer();
            //Command c1 = new BakeMuttonCommand(boy);
            //Command c2 = new BakeMuttonCommand(boy);
            //Command c3 = new BakeChickenWingCommand(boy);

            //Waiter girl = new Waiter();
            //girl.SetOrder(c1);
            //girl.SetOrder(c2);
            //girl.SetOrder(c3);

            //girl.Notify();

            #endregion

            #region 责任链模式
            ////使多个对象都有机会处理请求，
            ////从而避免请求的发送者和接收者的耦合关系。
            ////将这个对象连成一条链，并沿着这条链传递该请求，
            ////直到有一个对象处理它为止。
            //CommonManager jinli = new CommonManager("金立");
            //Majordomo zongjian = new Majordomo("宗建");
            //GeneralManager zongjingli = new GeneralManager("宗金丽");

            //jinli.SetSuperior(zongjian);
            //zongjian.SetSuperior(zongjingli);

            //Request request = new Request();
            //request.requestType = "请假";
            //request.requestContent = "小菜请假";
            //request.numBer = 1;
            //jinli.RequestApplications(request);

            //Request request2 = new Request();
            //request2.requestType = "请假";
            //request2.requestContent = "小菜请假";
            //request2.numBer = 1;
            //jinli.RequestApplications(request2);

            //Request request3 = new Request();
            //request3.requestType = "加薪";
            //request3.requestContent = "小菜请求加薪";
            //request3.numBer = 500;
            //jinli.RequestApplications(request3);

            //Request request4 = new Request();
            //request4.requestType = "加薪";
            //request4.requestContent = "小菜请求加薪";
            //request4.numBer = 1000;
            //jinli.RequestApplications(request4);
            #endregion

            #region 中介者模式
            ////用一个中介对象来封装一系列的对象交互，
            ////中介者使各对象不需要显式地互相调用，
            ////从而使其耦合松散，而且可以独立地改变它们之间的交互。

            //ConcreteMediator m = new ConcreteMediator();
            //ConcreteMediator1 c1 = new ConcreteMediator1(m);
            //ConcreteMediator2 c2 = new ConcreteMediator2(m);

            //m.Col1 = c1;
            //m.Col2 = c2;

            //c1.Send("发送消息");
            //c2.Send("收到消息");

            #endregion

            #region 享元模式
            //////运用共享技术有效地支持大量细粒度的对象。
            ////int ext = 22;

            ////FlyweightFactory f = new FlyweightFactory();

            ////Flyweight fx = f.GetFlyweight("X");
            ////fx.Operation(--ext);

            ////Flyweight fy = f.GetFlyweight("Y");
            ////fy.Operation(--ext);

            ////Flyweight fz = f.GetFlyweight("Z");
            ////fz.Operation(--ext);

            ////UnshareConcreteFlyweight uf = new UnshareConcreteFlyweight();
            ////uf.Operation(--ext);

            //WebSiteFactory f = new WebSiteFactory();

            //WebSite fx = f.GetWebSiteCategory("产品展示");
            //fx.Use();

            //WebSite fy = f.GetWebSiteCategory("产品展示");
            //fy.Use();

            //WebSite f1 = f.GetWebSiteCategory("博客");
            //f1.Use();

            //WebSite f2 = f.GetWebSiteCategory("博客");
            //f2.Use();

            //Console.WriteLine("网站分类总数为{0}",f.GetWebSiteCount());
            #endregion

            #region 解释器模式
            ////给定一个语言，定义它的文法的一种表示，
            ////并定义一个解释器，
            ////这个解释器使用该表示来解释语言中的句子。
            //Context1 context = new Context1();
            //IList<AbstractExpression> list = new List<AbstractExpression>();
            //list.Add(new TerminalExpression());
            //list.Add(new NoterminalExpression());
            //list.Add(new TerminalExpression());
            //list.Add(new TerminalExpression());

            //foreach (AbstractExpression exp in list)
            //{
            //    exp.Interpret(context);
            //}
            #endregion

            #region 访问者模式
            ////表示一个作用于某对象结构的各元素操作。
            ////它使你可以在不改变各元素的类的前提下
            ////定义作用于这些元素的新操作。
            //ObjectStructure o = new ObjectStructure();
            //o.Attach(new ConcreteElementA());
            //o.Attach(new ConcreteElementB());

            //ConcreteVisitorA v1 = new ConcreteVisitorA();
            //ConcreteVisitorB v2 = new ConcreteVisitorB();
            //o.Accept(v1);
            //o.Accept(v2);
            #endregion
        }
    }
}
