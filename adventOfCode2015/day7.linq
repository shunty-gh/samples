<Query Kind="Program" />

void Main()
{
	// Routine to solve day 7 of the AdventOfCode 2015
	// http://adventofcode.com/day/7

	//TestWires();
	//return ;

	var raw = GetRawInput();
	var instructions = GetInstructions(raw);
	//	instructions.Dump();
	//	instructions.Where(i => i.InstructionType == InstructionType.LeftShift).Count().Dump("LShift");
	//	instructions.Where(i => i.InstructionType == InstructionType.RightShift).Count().Dump("RShift");
	//	instructions.Where(i => i.InstructionType == InstructionType.AndGate).Count().Dump("And");
	//	instructions.Where(i => i.InstructionType == InstructionType.OrGate).Count().Dump("Or");
	//	instructions.Where(i => i.InstructionType == InstructionType.NotGate).Count().Dump("Not");
	//	instructions.Where(i => i.InstructionType == InstructionType.Wire).Count().Dump("Wire");
	//	instructions.Where(i => i.InstructionType == InstructionType.SimpleValue).Count().Dump("Value");

	// Process instructions
	Dictionary<string, Wire> wires = new Dictionary<string, Wire>();
	foreach (string wireId in instructions.Select(i => i.OutputId).OrderBy(w => w))
	{
		Wire wire = new Wire(wireId);
		wires.Add(wireId, wire);
	}
	
	//foreach (var instruction in instructions.Where(i => i.Input1Id == "bn" || i.Input2Id == "bn" || i.OutputId == "bn"))
	foreach (var instruction in instructions)
	{
		Wire output = wires[instruction.OutputId];
		
		SignalSource source = null;
		Wire wire;
		switch (instruction.InstructionType)
		{
			case InstructionType.LeftShift:
			case InstructionType.RightShift:
				source = MakeShiftGate(wires, instruction);
				break;
			case InstructionType.AndGate:
			case InstructionType.OrGate:
				source = MakeBinaryGate(wires, instruction);
				break;
			case InstructionType.NotGate:
				source = MakeNotGate(wires, instruction);
				break;
			case InstructionType.SimpleValue:
				source = new SimpleValueSignal(instruction.Input1Value);
				break;
			case InstructionType.Wire:
				source = wires[instruction.Input1Id];
				break;
			default:
				throw new Exception("Unhandled instruction type");
		}
		output.Input = source;
	}

	int wireA = wires["a"].Signal;
	wireA.Dump("Wire A");

	// Part 2
	// Feed the original answer for a into b
	wires["b"].Input = new SimpleValueSignal((ushort)wireA);
	// Reset the calculation status of each wire
	foreach (var wire in wires)
	{
		wire.Value.Reset();
	}
	// Re-calculate
	int wireA2 = wires["a"].Signal;
	wireA2.Dump("Wire A is now");
}

public void TestWires()
{
	/* Test run the example given on the page.
   	   Should get:
		x:	123
		y:	456
		d:	72
		e:	507
		f:	492
		g:	114
		h:	65412
		i:	65079
	*/

	var x = new Wire("x", new SimpleValueSignal(123));
	var y = new Wire("y", new SimpleValueSignal(456));
	List<Wire> wires = new List<Wire>
	{
		x,
		y,
		new Wire("d", new AndGate { Input1 = x, Input2 = y }),
		new Wire("e", new OrGate { Input1 = x, Input2 = y }),
		new Wire("f", new LShiftGate(2) { Input = x }),
		new Wire("g", new RShiftGate(2) { Input = y }),
		new Wire("h", new NotGate { Input = x }),
		new Wire("i", new NotGate { Input = y }),
	};

	foreach (var wire in wires)
	{
		wire.Signal.Dump(wire.Id);
	}
}

public OneInputGate MakeNotGate(Dictionary<string, Wire> wires, Instruction instruction)
{
	var gate = new NotGate();
	gate.Input = wires[instruction.Input1Id];
	return gate;
}

public TwoInputGate MakeBinaryGate(Dictionary<string, Wire> wires, Instruction instruction)
{
	TwoInputGate gate;
	if (instruction.InstructionType == InstructionType.AndGate)
	{
		gate = new AndGate();
	}
	else
	{
		gate = new OrGate();
	}
	SignalSource input1 = null, input2 = null;
	if (instruction.Input1Value > 0)
	{
		input1 = new SimpleValueSignal(instruction.Input1Value);
	}
	else 
	{
		input1 = wires[instruction.Input1Id];
	}
	gate.Input1 = input1;

	if (instruction.Input2Value > 0)
	{
		input2 = new SimpleValueSignal(instruction.Input2Value);
	}
	else 
	{
		input2 = wires[instruction.Input2Id];
	}
	gate.Input2 = input2;

	return gate;
}

public ShiftGate MakeShiftGate(Dictionary<string, Wire> wires, Instruction instruction)
{
	ShiftGate gate;
	if (instruction.InstructionType == InstructionType.LeftShift)
	{
		gate = new LShiftGate(instruction.Input2Value);
	}
	else
	{
		gate = new RShiftGate(instruction.Input2Value);
	}

	gate.Input = wires[instruction.Input1Id];
	return gate;
}

public abstract class SignalSource
{
	protected bool _isCalculated = false;
	public bool IsCalculated { get { return _isCalculated; } }
	protected int _signal = -1;
	public int Signal { get { return GetSignalValue(); } }

	protected int GetSignalValue()
	{
		if (!_isCalculated)
		{
			_signal = CalculateSignalValue();
			// Cache the value once calculated otherwise we'll be constantly re-calculating stuff and it will take forever
			_isCalculated = (_signal >= 0);
		}
		return _signal;
	}

	public virtual void Reset()
	{
		_isCalculated = false;
		//_signal = -1;
	}
	
	protected abstract int CalculateSignalValue();
}

public class SimpleValueSignal : SignalSource
{
	public SimpleValueSignal(ushort value)
	{
		this._signal = value;
		this._isCalculated = true;
	}

	protected override int CalculateSignalValue()
	{
		return _signal;
	}
}

public class Wire : SignalSource
{
	public string Id { get; set; }
	public SignalSource Input { get; set; }

	public Wire(string id)
	{
		this.Id = id;
	}

	public Wire(string id, SignalSource source)
		: this(id)
	{
		this.Input = source;
	}

	protected override int CalculateSignalValue()
	{
		return this.Input != null ? this.Input.Signal : -1;
	}

	public override void Reset()
	{
		if ((Input != null) && (Input.IsCalculated))
		{
			Input.Reset();
		}
		base.Reset();
	}
}

public abstract class Gate : SignalSource { }

public abstract class OneInputGate : Gate
{
	public SignalSource Input { get; set; }

	protected bool CheckInput()
	{
		return ((Input != null) && (Input.Signal >= 0));
	}

	public override void Reset()
	{
		if ((Input != null) && (Input.IsCalculated))
		{
			Input.Reset();
		}
		base.Reset();
	}
}

public abstract class TwoInputGate : Gate
{
	public SignalSource Input1 { get; set; }
	public SignalSource Input2 { get; set; }

	protected bool CheckInput()
	{
		return ((Input1 != null) && (Input2 != null) && Input1.Signal >= 0 && Input2.Signal >= 0);
	}

	public override void Reset()
	{
		if ((Input1 != null) && (Input1.IsCalculated))
		{
			Input1.Reset();
		}
		if (Input2 != null)
		{
			Input2.Reset();
		}
		base.Reset();
	}
}

public class AndGate : TwoInputGate
{
	protected override int CalculateSignalValue()
	{
		if (CheckInput())
		{
			return (ushort)((ushort)(Input1.Signal) & (ushort)(Input2.Signal));
		}
		return -1;
	}
}

public class OrGate : TwoInputGate
{
	protected override int CalculateSignalValue()
	{
		if (CheckInput())
		{
			return (ushort)((ushort)(Input1.Signal) | (ushort)(Input2.Signal));
		}
		return -1;
	}
}

public abstract class ShiftGate : OneInputGate
{
	protected int _shiftPlaces = 1;
	public int ShiftPlaces { get { return _shiftPlaces; } }

	public ShiftGate()
	{
	}

	public ShiftGate(int shiftPlaces)
		: this()
	{
		this._shiftPlaces = shiftPlaces;
	}
}

public class LShiftGate : ShiftGate
{
	public LShiftGate()
	{ }

	public LShiftGate(int shiftPlaces)
		: base(shiftPlaces)
	{ }

	protected override int CalculateSignalValue()
	{
		if (CheckInput())
		{
			return (ushort)((ushort)(Input.Signal) << _shiftPlaces);
		}
		return -1;
	}
}

public class RShiftGate : ShiftGate
{
	public RShiftGate()
	{ }

	public RShiftGate(int shiftPlaces)
		: base(shiftPlaces)
	{ }

	protected override int CalculateSignalValue()
	{
		if (CheckInput())
		{
			return (ushort)((ushort)(Input.Signal) >> _shiftPlaces);
		}
		return -1;
	}
}

public class NotGate : OneInputGate
{
	protected override int CalculateSignalValue()
	{
		if (CheckInput())
		{
			return (ushort)(~((ushort)(Input.Signal)));
		}
		return -1;
	}
}

public enum InstructionType
{
	Unknown,
	SimpleValue,
	Wire,
	LeftShift,
	RightShift,
	AndGate,
	OrGate,
	NotGate,
}

public class Instruction
{
	public InstructionType InstructionType { get; set; }
	public string Input1Id { get; set; }
	public string Input2Id { get; set; }
	public string OutputId { get; set; }
	public ushort Input1Value { get; set; }
	public ushort Input2Value { get; set; }

	public Instruction()
	{
		this.InstructionType = InstructionType.Unknown;
		this.Input1Id = "";
		this.Input2Id = "";
		this.OutputId = "";
		this.Input1Value = 0;
		this.Input2Value = 0;
	}
}

public IEnumerable<Instruction> GetInstructions(IEnumerable<string> raw)
{
	var result = new List<Instruction>();
	Regex re_gate = new Regex(@"(?<input1>\w+)\s+(?<gate>(([LR]SHIFT|AND|OR){1}))\s+(?<input2>\w+)\s+->\s+(?<out>\w+)");	
	Regex re_unary = new Regex(@"(?<not>(NOT)?)\s*(?<input1>\w+)\s+->\s+(?<out>\w+)");
	foreach (string str in raw)
	{
		Instruction instruction = new Instruction();

		string input1, input2;
		ushort value1, value2;
		if (re_gate.IsMatch(str))
		{
			var match = re_gate.Match(str);

			input1 = match.Groups["input1"].Value;
			input2 = match.Groups["input2"].Value;
			if (ushort.TryParse(input1, out value1))
			{
				instruction.Input1Value = value1;
			}
			else
			{
				instruction.Input1Id = input1;
			}
			if (ushort.TryParse(input2, out value2))
			{
				instruction.Input2Value = value2;
			}
			else
			{
				instruction.Input2Id = input2;
			}

			instruction.OutputId = match.Groups["out"].Value;
			string gate = match.Groups["gate"].Value;
			switch (gate)
			{
				case "LSHIFT":
					instruction.InstructionType = InstructionType.LeftShift;
					break;
				case "RSHIFT":
					instruction.InstructionType = InstructionType.RightShift;
					break;
				case "AND":
					instruction.InstructionType = InstructionType.AndGate;
					break;
				case "OR":
					instruction.InstructionType = InstructionType.OrGate;
					break;
			}
		}
		else if (re_unary.IsMatch(str))
		{
			var match = re_unary.Match(str);

			bool isnum = false;
			input1 = match.Groups["input1"].Value;
			if (ushort.TryParse(input1, out value1))
			{
				isnum = true;
				instruction.Input1Value = value1;
			}
			else
			{
				instruction.Input1Id = input1;
			}

			instruction.OutputId = match.Groups["out"].Value;
			if (match.Groups["not"].Value == "NOT")
			{
				instruction.InstructionType = InstructionType.NotGate;
			}
			else if (isnum)
			{
				instruction.InstructionType = InstructionType.SimpleValue;
			}
			else
			{
				instruction.InstructionType = InstructionType.Wire;
			}
		}
		result.Add(instruction);
	}
	return result;
}

public IEnumerable<string> GetRawInput()
{
	// The input from the question
	var input = @"bn RSHIFT 2 -> bo
lf RSHIFT 1 -> ly
fo RSHIFT 3 -> fq
cj OR cp -> cq
fo OR fz -> ga
t OR s -> u
lx -> a
NOT ax -> ay
he RSHIFT 2 -> hf
lf OR lq -> lr
lr AND lt -> lu
dy OR ej -> ek
1 AND cx -> cy
hb LSHIFT 1 -> hv
1 AND bh -> bi
ih AND ij -> ik
c LSHIFT 1 -> t
ea AND eb -> ed
km OR kn -> ko
NOT bw -> bx
ci OR ct -> cu
NOT p -> q
lw OR lv -> lx
NOT lo -> lp
fp OR fv -> fw
o AND q -> r
dh AND dj -> dk
ap LSHIFT 1 -> bj
bk LSHIFT 1 -> ce
NOT ii -> ij
gh OR gi -> gj
kk RSHIFT 1 -> ld
lc LSHIFT 1 -> lw
lb OR la -> lc
1 AND am -> an
gn AND gp -> gq
lf RSHIFT 3 -> lh
e OR f -> g
lg AND lm -> lo
ci RSHIFT 1 -> db
cf LSHIFT 1 -> cz
bn RSHIFT 1 -> cg
et AND fe -> fg
is OR it -> iu
kw AND ky -> kz
ck AND cl -> cn
bj OR bi -> bk
gj RSHIFT 1 -> hc
iu AND jf -> jh
NOT bs -> bt
kk OR kv -> kw
ks AND ku -> kv
hz OR ik -> il
b RSHIFT 1 -> v
iu RSHIFT 1 -> jn
fo RSHIFT 5 -> fr
be AND bg -> bh
ga AND gc -> gd
hf OR hl -> hm
ld OR le -> lf
as RSHIFT 5 -> av
fm OR fn -> fo
hm AND ho -> hp
lg OR lm -> ln
NOT kx -> ky
kk RSHIFT 3 -> km
ek AND em -> en
NOT ft -> fu
NOT jh -> ji
jn OR jo -> jp
gj AND gu -> gw
d AND j -> l
et RSHIFT 1 -> fm
jq OR jw -> jx
ep OR eo -> eq
lv LSHIFT 15 -> lz
NOT ey -> ez
jp RSHIFT 2 -> jq
eg AND ei -> ej
NOT dm -> dn
jp AND ka -> kc
as AND bd -> bf
fk OR fj -> fl
dw OR dx -> dy
lj AND ll -> lm
ec AND ee -> ef
fq AND fr -> ft
NOT kp -> kq
ki OR kj -> kk
cz OR cy -> da
as RSHIFT 3 -> au
an LSHIFT 15 -> ar
fj LSHIFT 15 -> fn
1 AND fi -> fj
he RSHIFT 1 -> hx
lf RSHIFT 2 -> lg
kf LSHIFT 15 -> kj
dz AND ef -> eh
ib OR ic -> id
lf RSHIFT 5 -> li
bp OR bq -> br
NOT gs -> gt
fo RSHIFT 1 -> gh
bz AND cb -> cc
ea OR eb -> ec
lf AND lq -> ls
NOT l -> m
hz RSHIFT 3 -> ib
NOT di -> dj
NOT lk -> ll
jp RSHIFT 3 -> jr
jp RSHIFT 5 -> js
NOT bf -> bg
s LSHIFT 15 -> w
eq LSHIFT 1 -> fk
jl OR jk -> jm
hz AND ik -> im
dz OR ef -> eg
1 AND gy -> gz
la LSHIFT 15 -> le
br AND bt -> bu
NOT cn -> co
v OR w -> x
d OR j -> k
1 AND gd -> ge
ia OR ig -> ih
NOT go -> gp
NOT ed -> ee
jq AND jw -> jy
et OR fe -> ff
aw AND ay -> az
ff AND fh -> fi
ir LSHIFT 1 -> jl
gg LSHIFT 1 -> ha
x RSHIFT 2 -> y
db OR dc -> dd
bl OR bm -> bn
ib AND ic -> ie
x RSHIFT 3 -> z
lh AND li -> lk
ce OR cd -> cf
NOT bb -> bc
hi AND hk -> hl
NOT gb -> gc
1 AND r -> s
fw AND fy -> fz
fb AND fd -> fe
1 AND en -> eo
z OR aa -> ab
bi LSHIFT 15 -> bm
hg OR hh -> hi
kh LSHIFT 1 -> lb
cg OR ch -> ci
1 AND kz -> la
gf OR ge -> gg
gj RSHIFT 2 -> gk
dd RSHIFT 2 -> de
NOT ls -> lt
lh OR li -> lj
jr OR js -> jt
au AND av -> ax
0 -> c
he AND hp -> hr
id AND if -> ig
et RSHIFT 5 -> ew
bp AND bq -> bs
e AND f -> h
ly OR lz -> ma
1 AND lu -> lv
NOT jd -> je
ha OR gz -> hb
dy RSHIFT 1 -> er
iu RSHIFT 2 -> iv
NOT hr -> hs
as RSHIFT 1 -> bl
kk RSHIFT 2 -> kl
b AND n -> p
ln AND lp -> lq
cj AND cp -> cr
dl AND dn -> do
ci RSHIFT 2 -> cj
as OR bd -> be
ge LSHIFT 15 -> gi
hz RSHIFT 5 -> ic
dv LSHIFT 1 -> ep
kl OR kr -> ks
gj OR gu -> gv
he RSHIFT 5 -> hh
NOT fg -> fh
hg AND hh -> hj
b OR n -> o
jk LSHIFT 15 -> jo
gz LSHIFT 15 -> hd
cy LSHIFT 15 -> dc
kk RSHIFT 5 -> kn
ci RSHIFT 3 -> ck
at OR az -> ba
iu RSHIFT 3 -> iw
ko AND kq -> kr
NOT eh -> ei
aq OR ar -> as
iy AND ja -> jb
dd RSHIFT 3 -> df
bn RSHIFT 3 -> bp
1 AND cc -> cd
at AND az -> bb
x OR ai -> aj
kk AND kv -> kx
ao OR an -> ap
dy RSHIFT 3 -> ea
x RSHIFT 1 -> aq
eu AND fa -> fc
kl AND kr -> kt
ia AND ig -> ii
df AND dg -> di
NOT fx -> fy
k AND m -> n
bn RSHIFT 5 -> bq
km AND kn -> kp
dt LSHIFT 15 -> dx
hz RSHIFT 2 -> ia
aj AND al -> am
cd LSHIFT 15 -> ch
hc OR hd -> he
he RSHIFT 3 -> hg
bn OR by -> bz
NOT kt -> ku
z AND aa -> ac
NOT ak -> al
cu AND cw -> cx
NOT ie -> if
dy RSHIFT 2 -> dz
ip LSHIFT 15 -> it
de OR dk -> dl
au OR av -> aw
jg AND ji -> jj
ci AND ct -> cv
dy RSHIFT 5 -> eb
hx OR hy -> hz
eu OR fa -> fb
gj RSHIFT 3 -> gl
fo AND fz -> gb
1 AND jj -> jk
jp OR ka -> kb
de AND dk -> dm
ex AND ez -> fa
df OR dg -> dh
iv OR jb -> jc
x RSHIFT 5 -> aa
NOT hj -> hk
NOT im -> in
fl LSHIFT 1 -> gf
hu LSHIFT 15 -> hy
iq OR ip -> ir
iu RSHIFT 5 -> ix
NOT fc -> fd
NOT el -> em
ck OR cl -> cm
et RSHIFT 3 -> ev
hw LSHIFT 1 -> iq
ci RSHIFT 5 -> cl
iv AND jb -> jd
dd RSHIFT 5 -> dg
as RSHIFT 2 -> at
NOT jy -> jz
af AND ah -> ai
1 AND ds -> dt
jx AND jz -> ka
da LSHIFT 1 -> du
fs AND fu -> fv
jp RSHIFT 1 -> ki
iw AND ix -> iz
iw OR ix -> iy
eo LSHIFT 15 -> es
ev AND ew -> ey
ba AND bc -> bd
fp AND fv -> fx
jc AND je -> jf
et RSHIFT 2 -> eu
kg OR kf -> kh
iu OR jf -> jg
er OR es -> et
fo RSHIFT 2 -> fp
NOT ca -> cb
bv AND bx -> by
u LSHIFT 1 -> ao
cm AND co -> cp
y OR ae -> af
bn AND by -> ca
1 AND ke -> kf
jt AND jv -> jw
fq OR fr -> fs
dy AND ej -> el
NOT kc -> kd
ev OR ew -> ex
dd OR do -> dp
NOT cv -> cw
gr AND gt -> gu
dd RSHIFT 1 -> dw
NOT gw -> gx
NOT iz -> ja
1 AND io -> ip
NOT ag -> ah
b RSHIFT 5 -> f
NOT cr -> cs
kb AND kd -> ke
jr AND js -> ju
cq AND cs -> ct
il AND in -> io
NOT ju -> jv
du OR dt -> dv
dd AND do -> dq
b RSHIFT 2 -> d
jm LSHIFT 1 -> kg
NOT dq -> dr
bo OR bu -> bv
gk OR gq -> gr
he OR hp -> hq
NOT h -> i
hf AND hl -> hn
gv AND gx -> gy
x AND ai -> ak
bo AND bu -> bw
hq AND hs -> ht
hz RSHIFT 1 -> is
gj RSHIFT 5 -> gm
g AND i -> j
gk AND gq -> gs
dp AND dr -> ds
b RSHIFT 3 -> e
gl AND gm -> go
gl OR gm -> gn
y AND ae -> ag
hv OR hu -> hw
1674 -> b
ab AND ad -> ae
NOT ac -> ad
1 AND ht -> hu
NOT hn -> ho";

	var result = input.Split('\n').ToList();
	return result;
}