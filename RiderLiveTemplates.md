# Setup

```
$mockName$.SetupSimple<$interfaceName$, $returnType$>(
    nameof($interfaceName$.$methodName$))
    .Returns($returnValue$);
$END$
```

# Setup Async

```
$mockName$.SetupSimpleAsync<$interfaceName$, $returnType$>(
    nameof($interfaceName$.$methodName$))
    .ReturnsAsync($returnValue$);
$END$
```

# Verify

```
$mockName$.VerifySimple(
    nameof($interfaceName$.$methodName$),
    Times.$times$);
$END$
```

# Verify Async

```
$mockName$.VerifySimpleAsync(
    nameof($interfaceName$.$methodName$),
    Times.$times$);
$END$
```